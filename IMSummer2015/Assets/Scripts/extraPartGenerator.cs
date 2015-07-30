using UnityEngine;
using System.Collections;

public class extraPartGenerator : MonoBehaviour {

	public Vector4[] generatorParameters; // (x,y,z,w) = (t.x, t.y, t.z, extraPart#)
	public bool initialized = false;
	// Put -999 for param if its not given/used
	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!initialized)
			initialize ();
	}

	void initialize()
	{
		initialized = true;
		var parameterArray = GetComponent<extraParams> ().parameters;
		Vector3 origin = transform.position;
		Vector3 newPos;
		GameObject gameObjectRef;
		for(int x=0;x<generatorParameters.Length;x++)
		{
			newPos = origin;
			if(check(generatorParameters[x].x))
				newPos.x = generatorParameters[x].x;
			if(check(generatorParameters[x].y))
				newPos.y = generatorParameters[x].y;
			if(check(generatorParameters[x].z))
				newPos.z = generatorParameters[x].z;
			gameObjectRef = GameObject.Find ("TrackManager").GetComponent<trackExtraPartPool>().pool[
			                                                  (int) parameterArray[(int) generatorParameters[x].w]];
			gameObjectRef = (GameObject) GameObject.Instantiate(gameObjectRef, transform.position, Quaternion.identity);
			gameObjectRef.transform.position = newPos + transform.position;
		}
		Destroy (this);
	}

	bool check(float param)
	{
		if ((int)param == -999)
			return false;
		else
			return true;
	}
}