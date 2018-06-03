using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car2dController : MonoBehaviour 
{
	// float speedForce = 10f;
	// float torqueForce = -200f;
	// float driftFactor = 0.99f;
	// // Use this for initialization
	// void Start () 
	// {
	// }
	// void FixedUpdate () 
	// {
	// 	Rigidbody2D rb = GetComponent<Rigidbody2D>();
	// 	if (Input.GetAxis("Vertical") > 0)
	// 	{
	// 		rb.AddForce(transform.up * speedForce);
	// 	}
	// 	rb.angularVelocity = Input.GetAxis("Horizontal") * torqueForce;
	// 	rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;
	// }

	// Vector2 ForwardVelocity() 
	// {
	// 	return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
	// }
	// Vector2 RightVelocity() 
	// {
	// 	return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
	// }
    // 
    // Source: https://answers.unity.com/questions/686025/top-down-2d-car-physics-1.html
    public float acceleration;
    public float steering;
    private Rigidbody2D rb;
 
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        Renderer renderer = gameObject.GetComponent<Renderer>();
        float width = renderer.bounds.size.x;
        Debug.Log("width " + width);
    }

    void FixedUpdate () {
        float h = -Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical"); 
        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);
 
        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if(direction >= 0.0f) 
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        } else 
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }
 
        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if(rb.angularVelocity > 0) 
        {
            steeringRightAngle = -90;
        } else 
        {
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