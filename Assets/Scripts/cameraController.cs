using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	
	public float positionDampTime = 0.15f;
	public float rotationDampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

	// Update is called once per frame
	void LateUpdate () 
	{
		Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
		Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.3f, point.z)); //(new Vector3(0.5, 0.5, point.z));
		Vector3 destinationPosition = transform.position + delta;
		Debug.Log(velocity.x * velocity.y + " ");
		transform.position = Vector3.SmoothDamp(transform.position, destinationPosition, ref velocity, positionDampTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationDampTime);
	}
}