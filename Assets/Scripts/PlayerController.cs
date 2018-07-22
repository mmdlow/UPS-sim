using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    GameManager gameManager;

    public delegate void OnSustainDamage(int damage);
    public OnSustainDamage onSustainDamageCallback;

    public float acceleration;
    public float steering;
    public float damageConstant = 2f;
    public int health;
    public Text healthDisplay;
    public Text damageDisplay;
    private Rigidbody2D rb;
    public static GameObject instance;
    public GameObject directionIndicatorPrefab;
    private GameObject directionIndicator;

	void Awake() {
		if (instance == null) {
			instance = this.gameObject;
		} else if (instance != this) {
			Destroy(this.gameObject); // enforce singleton pattern 
		}

        directionIndicator = Instantiate(directionIndicatorPrefab);
        directionIndicator.transform.parent = this.gameObject.transform;
	}

    void Start () {
        health = BoardManager.instance.health; // Use health field in game manager
        rb = GetComponent<Rigidbody2D>();
        healthDisplay.text = health.ToString();
    }  

    void Update() {
    }

    IEnumerator DisplayDamage () {
        damageDisplay.CrossFadeAlpha(1, 0f, false);
        yield return new WaitForSeconds(2);
        damageDisplay.CrossFadeAlpha(0, 1f, false);
    }

    void OnCollisionEnter2D (Collision2D col) {
        int damage = Mathf.RoundToInt(damageConstant * col.relativeVelocity.magnitude);
        health -= damage;

        // For updating item integrities upon sustaining player damage
        if (onSustainDamageCallback != null) {
            onSustainDamageCallback(damage);
        }

        if (damage > 0) {
            damageDisplay.text = (-damage).ToString();
            StartCoroutine(DisplayDamage());
        }

        if (health <= 0) {
            healthDisplay.text = "0";
            BoardManager.instance.GameOver();
            return;
        }
        if (health < 100 && health > 25) healthDisplay.color = Color.white;
        else if (health <= 25) healthDisplay.color = Color.red;
        healthDisplay.text = health.ToString();
    }

    void FixedUpdate () {
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

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
    }
}