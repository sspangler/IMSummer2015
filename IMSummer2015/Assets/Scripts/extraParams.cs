using UnityEngine;
using System.Collections;

public class extraParams : MonoBehaviour {

	public float[] parameters;
	public Vector3[] properties; // Vec3(min, max, increment)

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		roundEverything ();
	}

	public float inc(int index, float previousValue = -9999f)
	{
		float newValue = previousValue;
		if(previousValue==-9999f)
			newValue = parameters[index];

		newValue += properties [index].z;
		if (newValue > properties [index].y)
			newValue = properties [index].y;
		parameters [index] = newValue;
		return newValue;
	}

	public float dec(int index, float previousValue = -9999f)
	{
		float newValue = previousValue;
		if(previousValue==-9999f)
			newValue = parameters[index];
		
		newValue += -properties [index].z;
		if (newValue < properties [index].x)
			newValue = properties [index].x;
		parameters [index] = newValue;
		return newValue;
	}

	void roundEverything()
	{
		float roundTo = 1f;
		// partParameters[(int)paramReferences[x].w].w
		for(int x=0;x<parameters.Length;x++)
		{
			parameters[x] = Mathf.Round(parameters[x] * (1f / roundTo)) * (roundTo / 1f);
		}
	}
}
