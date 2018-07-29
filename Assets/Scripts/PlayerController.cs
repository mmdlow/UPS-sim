using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    GameManager gameManager;

    public delegate void OnSustainDamage(float damage);
    public OnSustainDamage onSustainDamageCallback;

    public float acceleration;
    public float steering;
    public float damageConstant = 3f;
    public Text healthDisplay;
    public Text damageDisplay;
    private Rigidbody2D rb;
    public static PlayerController instance;
    public GameObject directionIndicatorPrefab;
    public GameObject directionIndicator;
    public PlayerShooter playerShooter;
    private GameObject dropzone;
    private float CHANGE_INDICATOR_DISTANCE = 8f;
    public AudioSource engineSound;
    public AudioClip impactSound1;
    public AudioClip impactSound2;
    public AudioClip impactSound3;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this.gameObject); // enforce singleton pattern 
		}

        directionIndicator = Instantiate(directionIndicatorPrefab);
        directionIndicator.transform.parent = this.gameObject.transform;
	}

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        healthDisplay.text = GameManager.instance.GetHealth().ToString();
        ItemManager.instance.onPriorityItemChange += UpdatePriorityItem;
		playerShooter = GetComponent<PlayerShooter>();
    }  

    void Update() {
    }

    IEnumerator DisplayDamage () {
        damageDisplay.CrossFadeAlpha(1, 0f, false);
        yield return new WaitForSeconds(2);
        damageDisplay.CrossFadeAlpha(0, 1f, false);
    }

    void OnCollisionEnter2D (Collision2D col) {

        SoundManager.instance.RandomSfx(impactSound1, impactSound2, impactSound3);

        int damage = Mathf.RoundToInt(damageConstant * col.relativeVelocity.magnitude);
        MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.TAKINGDAMAGE, 5);
        int health = GameManager.instance.GetHealth();
        health = GameManager.instance.SetHealth(health - damage);

        // For updating item integrities upon sustaining player damage
        if (onSustainDamageCallback != null) {
            onSustainDamageCallback(col.relativeVelocity.magnitude);
        }

        if (damage > 0) {
            damageDisplay.text = (-damage).ToString();
            StartCoroutine(DisplayDamage());
        }

        if (health <= 0) {
            healthDisplay.text = "0";
            GameManager.instance.GameOver();
        } else {
            if (health <= 100 && health > 25) {
                healthDisplay.color = Color.white;
            }
            else {
                healthDisplay.color = Color.red;
                MessageManager.instance.SayPreparedMessage(MessageManager.PreparedMessage.LOWHEALTH, 10);
            }
            healthDisplay.text = health.ToString();
        } 
    }

    void FixedUpdate () {
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        // Engine sound
        if (engineSound.clip != null) engineSound.pitch = 1 + v / 2;

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f) {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        } else {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0) {
            steeringRightAngle = -90;
        } else {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce));

        ShowPlayerShooterOrDirectionIndicator();
    }

    void ShowPlayerShooterOrDirectionIndicator() {
        if (dropzone == null) {
            directionIndicator.SetActive(false);
            playerShooter.enabled = false;
        } else {
            // show which depending on distance
            if (Vector3.Distance(transform.position, dropzone.transform.position) > CHANGE_INDICATOR_DISTANCE) {
                // DIC should be shown if not already
                if (!directionIndicator.activeInHierarchy) {
                    directionIndicator.SetActive(true);
                    playerShooter.enabled = false;
                }
            } else {
                // PS should be shown if not already
                if (!playerShooter.enabled) {
                    playerShooter.enabled = true;
                    directionIndicator.SetActive(false);
                }
            }
        }
    }
	void UpdatePriorityItem(GameObject priorityItem) {
		if (priorityItem == null) {
			dropzone = null;
		} else {
			dropzone = priorityItem.transform.GetChild(0).gameObject;
		}
	}
}