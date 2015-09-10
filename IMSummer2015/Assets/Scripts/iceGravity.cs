using UnityEngine;
using System.Collections;

public class iceGravity : MonoBehaviour {

	//public float gravityEffect = 10f; // in m/s? probably close enough (earth=9.8)
	//public float terminalVelocity = 40f; // human is 56
	//public float curveExponent = 2f; // Similar to drag? Lower number = higher drag.
	public bool jumpEnabled = false;
	public float jumpPower = 1f;
	public float gravityPower;
	public float maxGravity;
	public float maxHoverTime;
	public Vector2 hoverDrag;
	public float hangtime = 6f;
	bool jumping = false;
	float jumpTime = 0f;
	float targetJumpTime;
	float hoverTime = 0f;
	public bool falling = false;
	public float velocity = 0f;
	iceMove moveRef;
	float fallingTime = 0f;
	float fallingFrame;
	bool recentFall = false;
	float spaceDown = 0f;
	float maxSpaceDown = 0.1f;

	// Use this for initialization
	void Awake () {
		fallingFrame = 1f / 60f;
		hangtime = hangtime * fallingFrame;
		maxHoverTime = maxHoverTime * fallingFrame;
		maxSpaceDown = maxSpaceDown * fallingFrame;
		moveRef = GetComponent<iceMove> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (spaceDown > 0f)
			spaceDown += -Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Space))
			spaceDown = maxSpaceDown;

		if(!falling)
		{
			jumping = false;
			fallingTime = 0f;
			if(!recentFall)
				velocity = (moveRef.bodyLength - moveRef.lastRayDistance) / Time.deltaTime;
			else velocity = 0f;

			// Jumpable (adds velocity)
			if((spaceDown>0)&&jumpEnabled&&!recentFall)
			{
				falling = true;
				jumping = true;
				velocity = jumpPower;
				jumpTime = 0f;
				targetJumpTime = 10f;
				transform.position += new Vector3(0f, velocity*fallingFrame, 0f);
			}
			else if(moveRef.lastRayDistance> moveRef.bodyLength)
			{
				falling = true;
			}
			recentFall = false;
		}
		else
		{
			recentFall = true;
			if(moveRef.lastRayDistance<moveRef.bodyLength)
			{
				falling = false;
				jumping = false;
			}

			fallingTime += Time.deltaTime;
			// Apply any and all missed frames of gravity physics
			while(true)
			{
				if(fallingTime>=fallingFrame)
				{
					fallingTime += -fallingFrame;
					applyGravity();
				}
				else break;
			}

			if(jumping)
			{
				jumpTime += Time.deltaTime;
				targetJumpTime = hangtime;
				if(jumpTime>targetJumpTime)
				{
					jumping = false;
					hoverTime = maxHoverTime;
				}
			}
		}
	}

	void applyGravity()
	{
		float debugA = velocity;
		if(!jumping)
		{
			/*if (velocity >= 0f)
				velocity += -gravityEffect*(1f)*fallingFrame;//+(returnFactor(Mathf.Abs(velocity), 50f, curveExponent, true)))*fallingFrame;
			else
			{
				velocity += -(gravityEffect*returnFactor(Mathf.Abs(velocity), terminalVelocity, curveExponent, false)*fallingFrame);
			}*/
			if(!Input.GetKey(KeyCode.Space))
				hoverTime = 0f;

			if(velocity >= 0f && hoverTime> 0f)
			{
				velocity += -(hoverDrag.x * fallingFrame + (hoverDrag.y*velocity));
				if(velocity<0f)
					velocity = 0f;
				hoverTime += -fallingFrame;
				Debug.Log ("Hovering");
			}
			else
			{
				velocity += -gravityPower*fallingFrame;
			}
		}
		//Debug.Log ("Vel Change: " + (debugA - velocity));
		transform.position += new Vector3(0f, velocity*fallingFrame, 0f);
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