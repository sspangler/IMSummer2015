using UnityEngine;
using System.Collections;

public class rotationControls : MonoBehaviour {

	public float thrusterValue = 500f;
	public float xPower;
	public float yPower;
	public float zPower;

	public GameObject[] thrusters;

	public Vector3 velocityClamps = new Vector3(20f, 50f, 0f);

	public bool strafer = false;

	public float strafeThrusterPower = 30f;
	public GameObject leftThruster;
	public GameObject rightThruster;

	Vector3 eulers;

	float clampSize;

	// Use this for initialization
	void Awake () {
		clampSize = velocityClamps.y - velocityClamps.x;
		eulers = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(transform.position.y<3f)
			GameObject.Find("TrackManager").GetComponent<trackManager>().resetHoverplaneTransform();
	}

	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Mouse1))
			GetComponent<Rigidbody> ().drag = 999f;
		float newThrusterValue = 0f;
		if (Input.GetKey(KeyCode.Mouse0))
			newThrusterValue = thrusterValue;
		foreach(GameObject t in thrusters)
		{
			t.GetComponent<thruster>().power = newThrusterValue;
		}

		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		if (strafer) 
		{
			if(Input.GetKey(KeyCode.A))
				rigidbody.velocity = new Vector3(-1 * xPower, rigidbody.velocity.y, rigidbody.velocity.z);
			else if(Input.GetKey(KeyCode.D))
				rigidbody.velocity = new Vector3(1 * xPower, rigidbody.velocity.y, rigidbody.velocity.z);
			else rigidbody.velocity = new Vector3(0 * xPower, rigidbody.velocity.y, rigidbody.velocity.z);
		} 
		else 
		{
			//Debug.Log (rigidbody.angularVelocity.y);
			//eulers += new Vector3(-returnMouseY () * xPower * Time.fixedDeltaTime, returnMouseX () * yPower * Time.fixedDeltaTime, -returnRot () * zPower * Time.fixedDeltaTime);
			//transform.localEulerAngles = eulers;
			//rigidbody.AddTorque(-returnMouseY () * xPower * Time.fixedDeltaTime, returnMouseX () * yPower * Time.fixedDeltaTime, -returnRot () * zPower * Time.fixedDeltaTime);
			//rigidbody.AddRelativeTorque (0f, returnRot () * yPower * Time.fixedDeltaTime, 0f);
			rigidbody.angularVelocity = new Vector3 (0f, Input.GetAxis ("Horizontal") * yPower, 0f);
		}
	}

	float mouseXClamp = 5f;
	float returnMouseX()
	{
		return Mathf.Clamp (Input.GetAxis ("Mouse X"), -mouseXClamp, mouseXClamp) / mouseXClamp;
	}

	float mouseYClamp = 3f;
	float returnMouseY()
	{
		return Mathf.Clamp (Input.GetAxis ("Mouse Y"), -mouseYClamp, mouseYClamp) / mouseYClamp;
	}

	float rotClamp = 1f;
	float returnRot()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		if(Input.GetAxis ("Horizontal")>0f && rigidbody.angularVelocity.y>velocityClamps.x)
		{
			return Mathf.Clamp (Input.GetAxis ("Horizontal"), 0f, rotClamp-(rotClamp*(rigidbody.angularVelocity.y-velocityClamps.x/clampSize))) / rotClamp;
		}
		else if(Input.GetAxis ("Horizontal")<0f && rigidbody.angularVelocity.y<-velocityClamps.x)
		{
			return Mathf.Clamp (Input.GetAxis ("Horizontal"), -rotClamp+(rotClamp*(-rigidbody.angularVelocity.y-velocityClamps.x/clampSize)), 0f) / rotClamp;
		}
		else return Mathf.Clamp (Input.GetAxis ("Horizontal"), -rotClamp, rotClamp) / rotClamp;
	}
}
