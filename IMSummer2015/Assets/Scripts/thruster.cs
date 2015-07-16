using UnityEngine;
using System.Collections;

public class thruster : MonoBehaviour {

	public float velocity = 0f;
	public float acceleration = 0f;
	public float power = 0f;

	// Limiters: From x to y speed is reduced by z
	public Vector3[] limiters;
	public bool logVelocity = false;
	public bool logLimiter = false;
	Rigidbody myRigidbody;
	float currentLimiter = 1f;

	// Use this for initialization
	void Awake () {
		GameObject currentGO = gameObject;
		while(true)
		{
			if(currentGO.GetComponent<Rigidbody>())
			{
				myRigidbody = currentGO.GetComponent<Rigidbody>();
				break;
			}
			else
			{
				if(currentGO.transform.parent!=null)
					currentGO = currentGO.transform.parent.gameObject;
				else
				{
					Debug.Log ("No Rigidbody found");
					break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		velocity = Vector3.Magnitude(myRigidbody.velocity);
		if(logVelocity)
			Debug.Log ("Velocity: " + velocity);
		updateLimiter ();
		acceleration = Vector3.Magnitude(myRigidbody.velocity) - velocity;
		myRigidbody.AddForce (transform.forward*power*currentLimiter);
	}

	void updateLimiter()
	{
		currentLimiter = 1f;
		float currentSpeed = 0f;
		float angle = Mathf.Abs (Vector3.Angle (transform.forward, myRigidbody.velocity));
		if(angle<90f)
		{
			currentSpeed = velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
		}

		float a;
		float factor;
		foreach(Vector3 limit in limiters)
		{
			if(currentSpeed>limit.x)
			{
				if(currentSpeed>limit.y)
					factor = 1f;
				else
					factor = (currentSpeed - limit.x) / (limit.y - limit.x);
				currentLimiter += -(limit.z * factor);
			}
		}
		if (logLimiter)
			Debug.Log (currentLimiter);
	}
}