using UnityEngine;
using System.Collections;

public class hoverFollow : MonoBehaviour {

	public GameObject toFollow;
	public float yValue = 3f;
	public float distance = 6f;
	public float zoomRate = 0.5f; // recommended = yValue / distance
	float velocity;
	Vector3 lastPosition;

	float maxSpeed = 170f;
	float minSpeed = 10f;

	// Use this for initialization
	void Awake () {
		lastPosition = toFollow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//velocity = Vector3.Magnitude(toFollow.GetComponent<Rigidbody>().velocity);
		lastPosition = toFollow.transform.position;
		//float ratioFactor = (velocity-minSpeed)/(maxSpeed-minSpeed);
		//ratioFactor = zoomRate * (returnFactor(Mathf.Clamp (ratioFactor, 0f, 1f), 1f, 2f, true)) + 1f;
		transform.position = toFollow.transform.position;
		transform.eulerAngles = new Vector3 (0f, toFollow.transform.eulerAngles.y, 0f);
		//transform.position += -transform.forward * distance * ratioFactor;
		//transform.position += new Vector3 (0f, yValue * ratioFactor, 0f);
		transform.LookAt (toFollow.transform.position);
	}

	float returnFactor(float a, float b, float exponent, bool inverseExponent)
	{
		float factor = 0f;
		factor = a / b;
		if(!inverseExponent)
		{
			factor = Mathf.Pow(factor, exponent);
		}
		else
		{
			factor = 1f - (Mathf.Pow(1f-factor, exponent));
		}
		
		return factor;
	}
}
