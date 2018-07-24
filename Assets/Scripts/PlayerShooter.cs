using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour {

	private Rigidbody2D rb;
	public GameObject projectilePrefab;
	public float PROJ_VELOCITY = 5f;
	private bool fired = false;
	private bool firing = false;
	private GameObject priorityItem;
    private GameObject directionIndicator;
	private Image powerBarImage;
	private float fillAmount = 0f;
	private float deltaFill = 0.02f;
	private bool upward = true;
	private LineRenderer line;
	private GameObject dropzone;
	private Vector3[] positions;
	public Material material;
	private float angle;
	private Vector3 outerVector;
	private Vector3 innerVector;
	void Start () {
        rb = GetComponent<Rigidbody2D>();
		ItemManager.instance.onPriorityItemChange += UpdateCurrentItem;
		powerBarImage = GameObject.Find("PowerBar").GetComponent<Image>();
		powerBarImage.enabled = false;

		line = gameObject.AddComponent<LineRenderer>();
       	line.startWidth = 0.05F;
        line.endWidth = 0.05F;
		line.useWorldSpace = true;
		positions = new Vector3[3];
		line.positionCount = positions.Length;
        float alpha = 1.0f;
		line.material = new Material(material);
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), 
									new GradientColorKey(Color.red, 1.0f), 
									new GradientColorKey(Color.green, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
		line.sortingLayerName = "Buildings";
        line.colorGradient = gradient;
	}
	void OnEnable() {
		line.enabled = true;
	}
	void OnDisable() {
		line.enabled = false;
	}
	void UpdateLine(float angle) {
		if (priorityItem == null) return;
		outerVector = FindOuterVector(angle/2, transform.position, dropzone.transform.position);
		innerVector = FindInnerVector(angle/2, transform.position, dropzone.transform.position);
		positions[0] = outerVector;
		positions[1] = transform.position;
		positions[1].z = -1;
		positions[2] = innerVector;
		line.SetPositions(positions);
	}

	/*
			     _______.  ______ .______       ___________    __    ____      ______      __    __       ___   .___________. _______ .______      .__   __.  __    ______   .__   __.      _______.
    /       | /      ||   _  \     |   ____\   \  /  \  /   /     /  __  \    |  |  |  |     /   \  |           ||   ____||   _  \     |  \ |  | |  |  /  __  \  |  \ |  |     /       |
   |   (----`|  ,----'|  |_)  |    |  |__   \   \/    \/   /     |  |  |  |   |  |  |  |    /  ^  \ `---|  |----`|  |__   |  |_)  |    |   \|  | |  | |  |  |  | |   \|  |    |   (----`
    \   \    |  |     |      /     |   __|   \            /      |  |  |  |   |  |  |  |   /  /_\  \    |  |     |   __|  |      /     |  . `  | |  | |  |  |  | |  . `  |     \   \    
.----)   |   |  `----.|  |\  \----.|  |____   \    /\    /       |  `--'  '--.|  `--'  |  /  _____  \   |  |     |  |____ |  |\  \----.|  |\   | |  | |  `--'  | |  |\   | .----)   |   
|_______/     \______|| _| `._____||_______|   \__/  \__/         \_____\_____\\______/  /__/     \__\  |__|     |_______|| _| `._____||__| \__| |__|  \______/  |__| \__| |_______/    

				Back to Trigonometry 101

							*  <-- endpoint    }
						   /|  }			   }
						  / |  }			   }
						 /  |  } deltaY3       }
						/	|  }               } 
					   /	|  }               }
					  /	    *  <-- endpoint2   }  deltaY
					 /	    |  }               }
					/		|  }               }
				   /		|  } deltaY2       }
				  /		    |  }               }
				 /			|  }               }
  startpoint--> *-----------* ------>X
				^^^^^^^^^^^^^
				    deltaX
			<theta = angle of endpoint to startpoint to X
			<alpha = angle of endpoint to startpoint to endpoint2
	 */
	Vector3 FindInnerVector(float alpha, Vector3 startPoint, Vector3 endPoint) {
		alpha = (Mathf.PI/180) * alpha; // convert alpha to radians
		float deltaY = Mathf.Abs(startPoint.y - endPoint.y); // height of triangle
		float deltaX = Mathf.Abs(startPoint.x - endPoint.x); // base of triangle
		float theta = Mathf.Atan2(deltaY, deltaX);
		float deltaY2 = deltaX * Mathf.Tan(theta - alpha);
		float newY = endPoint.y > startPoint.y ? startPoint.y + deltaY2 : startPoint.y - deltaY2;
		return new Vector3(endPoint.x, newY, -1);
	}

	Vector3 FindOuterVector(float alpha, Vector3 startPoint, Vector3 endPoint) {
		alpha = (Mathf.PI/180) * alpha; // convert alpha to radians
		float deltaY = Mathf.Abs(startPoint.y - endPoint.y); // height of triangle
		float deltaX = Mathf.Abs(startPoint.x - endPoint.x); // base of triangle
		float theta = Mathf.Atan2(deltaX, deltaY);
		float deltaX2 = deltaY * Mathf.Tan(theta - alpha);
		float newX = endPoint.x > startPoint.x ? startPoint.x + deltaX2 : startPoint.x - deltaX2;
		return new Vector3(newX, endPoint.y, -1);
	}

    Vector3 FindRandomVectorInBetween(float angle, Vector3 startPoint, Vector3 endPoint) {
        float newAlpha = Random.Range(0, angle / 2);
        bool outside = Random.value > 0.5f;
        Vector3 newVector = outside ? FindOuterVector(newAlpha, startPoint, endPoint) : FindInnerVector(newAlpha, startPoint, endPoint);
		return newVector;
    }
	
	void FixedUpdate () {
		angle = (rb.velocity.magnitude/4f)*120f;
		if (!firing && Input.GetButton("Shoot") && fired == false) {
			// Begin firing sequence
            firing = true;
            fillAmount = 0f;
			powerBarImage.enabled = true;
		}
		if (firing) {
			UpdateFillAmount();
            if (!Input.GetButton("Shoot")) {
                // End firing sequence
                Shoot(priorityItem, 
					  powerBarImage.fillAmount,
					  FindRandomVectorInBetween(angle, transform.position, dropzone.transform.position) - transform.position
				);
                firing = false;
                powerBarImage.enabled = false;
            } else {
				powerBarImage.fillAmount = fillAmount;
			}
		}
		UpdateLine(angle);
	}

	// Ensure that Type in Image component is set to Filled
	void UpdateFillAmount() {
		if (upward) {
            if (fillAmount + deltaFill < 1f) {
                fillAmount += deltaFill;
            }
            else {
                // switch directions
				upward = false;
            }
		} else {
			// downward
			if (fillAmount - deltaFill > 0) {
				fillAmount -= deltaFill;
			} else {
				// switch directions
				upward = true;
			}
		}
	}

	// power is between 0 and 1
	void Shoot(GameObject item, float power, Vector3 direction) {
		GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
		projectile.GetComponent<ProjectileController>().SetItem(item);
		Rigidbody2D prb = projectile.GetComponent<Rigidbody2D>();
        prb.velocity = PROJ_VELOCITY * power * direction;
	}

	void UpdateCurrentItem(GameObject priorityItem) {
		this.priorityItem = priorityItem;
		this.dropzone = priorityItem.GetComponent<ItemController>().dropzone;
	}
}
