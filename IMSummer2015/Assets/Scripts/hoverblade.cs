using UnityEngine;
using System.Collections;

public class hoverblade : MonoBehaviour {

	public bool onlyFloat = false;
	public float power;
	public float constantPower;
	public float maxDistance = 10f;
	public float distanceExponent = 2f;
	public bool distanceInverseExponent = false;
	public float normalExponent = 2f;
	public bool normalInverseExponent = true;
	public GameObject originObject;
	public bool debugLog = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 origin = originObject.transform.position;
		RaycastHit hit;
		Ray ray = new Ray (origin, -transform.up);
		if (Physics.Raycast(ray, out hit)) {
			Vector3 incomingVec = hit.point - origin;
			Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
			Debug.DrawLine(origin, hit.point, Color.red);
			Debug.DrawRay(hit.point, reflectVec, Color.green);
			Debug.DrawLine(origin, origin+(-transform.up*maxDistance), Color.blue);
			// Apply Force
			float strengthFactor = 1f;
			float distanceFactor;
			if(hit.distance<maxDistance)
				distanceFactor = returnFactor(maxDistance-hit.distance, maxDistance,
			                                    distanceExponent, distanceInverseExponent);
			else distanceFactor = 0f;
			float normalFactor = 1f - returnFactor(Vector3.Angle(origin-hit.point, reflectVec), 180f,
			                                  normalExponent, normalInverseExponent);
			if(onlyFloat)
				GetComponent<Rigidbody>().AddForce(0f, (constantPower+power)*strengthFactor*normalFactor*distanceFactor*Time.fixedDeltaTime, 0f);
			else
			GetComponent<Rigidbody>().AddRelativeForce(0f, (constantPower+power)*strengthFactor*normalFactor*distanceFactor*Time.fixedDeltaTime, 0f);
		}
		else GetComponent<Rigidbody>().AddRelativeForce(0f, constantPower*Time.fixedDeltaTime, 0f);
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
