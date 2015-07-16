using UnityEngine;
using System.Collections;

public class dynamicDrag : MonoBehaviour {

	public Vector3[] dragPoints;

	// Vector3[0] = The direction of the drag.
	// Vector3[1] = (angle range, effect power, patternn)
	// Patterns:
	// 0 = Linear
	// <0 = Exponential

	public float defaultDrag;

	// Use this for initialization
	void Awake () {
		defaultDrag = GetComponent<Rigidbody> ().drag;
		for(int x=0;x<dragPoints.Length;x++)
		{
			dragPoints[x] = Vector3.Normalize(dragPoints[x]);
			x++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody>().drag = updateDrag();
		//Debug.Log (Vector3.Angle (GetComponent<Rigidbody> ().velocity, transform.right));
	}

	float updateDrag()
	{
		float newDrag = defaultDrag;
		Vector3 a;
		Vector3 b;
		Vector3 direction = Vector3.Normalize(GetComponent<Rigidbody>().velocity);

		float proximity;

		for(int x=0;x<dragPoints.Length;x++)
		{
			a = dragPoints[x];
			b = dragPoints[x+1];

			proximity = Vector3.Angle(direction, a);

			if(proximity<b.x)
			{
				float strengthFactor = proximity / b.x;
				strengthFactor = 1f - strengthFactor;
				if(b.z==0f)
				{
					newDrag += strengthFactor * b.y;
				}
				else if(b.z<0f)
				{
					//strengthFactor = Mathf.Pow(strengthFactor, Mathf.Abs(b.z));
					strengthFactor = returnFactor(strengthFactor, 1f, Mathf.Abs(b.z), false);
					newDrag += strengthFactor * b.y;
				}
			}
			x++;
		}
		return newDrag;
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
