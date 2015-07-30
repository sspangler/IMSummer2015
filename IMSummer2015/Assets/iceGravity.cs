using UnityEngine;
using System.Collections;

public class iceGravity : MonoBehaviour {

	public float gravityEffect = 7f; // in m/s? probably close enough (earth=9.8)
	public float terminalVelocity = 40f; // human is 56
	public float curveExponent = 2f; // Similar to drag? Lower number = higher drag.
	public bool falling = false;
	public float velocity = 0f;
	iceMove moveRef;
	float fallingTime = 0f;
	float fallingFrame;

	// Use this for initialization
	void Awake () {
		fallingFrame = 1f / 60f;
		moveRef = GetComponent<iceMove> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!falling)
		{
			fallingTime = 0f;
			velocity = (moveRef.bodyLength - moveRef.lastRayDistance) / Time.deltaTime;

			// Jumpable (adds velocity)

			if(moveRef.lastRayDistance> moveRef.bodyLength)
			{
				falling = true;
				Debug.Log("Now falling");
			}
		}
		else
		{
			if(moveRef.lastRayDistance<moveRef.bodyLength)
			{
				falling = false;
				Debug.Log("Now skating");
			}

			fallingTime += Time.deltaTime;
			while(true)
			{
				if(fallingTime>=fallingFrame)
				{
					fallingTime += -fallingFrame;
					applyGravity();
				}
				else break;
			}
			transform.position += new Vector3(0f, velocity*fallingFrame, 0f);
		}
	}

	void applyGravity()
	{
		if (velocity > 0f)
			velocity += -gravityEffect*fallingFrame;
		else
		{
			velocity += -(gravityEffect*returnFactor(Mathf.Abs(velocity), terminalVelocity, curveExponent, false)*fallingFrame);
		}
	}

	// Copied from elsewhere. Returns a factor of EXPONENT based on curve from 0 to 1.0
	float returnFactor(float a, float b, float exponent, bool inverseLine = false)
	{
		float factor = a / b;
		if (b > a)
			factor = 1f - (a / b); // If you put the larger # first, it flips the graph over the y line
		if(!inverseLine)
		{
			factor = Mathf.Pow(factor, exponent);
		}
		else
		{
			factor = 1f - (Mathf.Pow(factor, exponent));
		}
		
		return factor;
	}
}