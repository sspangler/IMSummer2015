using UnityEngine;
using System.Collections;

public class extraPartGenerator : MonoBehaviour {

	public Vector4[] generatorParameters; // (x,y,z,w) = (t.x, t.y, t.z, extraPart#)
	public bool initialized = false;
	extraParams paramRef;
	trackExtraPartPool extraPoolRef;
	// Put -999 for param if its not given/used
	// Use this for initialization
	void Awake () {
		paramRef = GetComponent<extraParams> ();
		if (GameObject.Find ("TrackManager").GetComponent<trackExtraPartPool> ())
			extraPoolRef = GameObject.Find ("TrackManager").GetComponent<trackExtraPartPool> ();
		else extraPoolRef = GameObject.Find ("GameManager").GetComponent<trackExtraPartPool>();
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
			Debug.Log("Spawned");
			newPos = origin;
			if(check(generatorParameters[x].x))
				newPos.x = paramRef.parameters[(int)generatorParameters[x].x];
			if(check(generatorParameters[x].y))
				newPos.y = paramRef.parameters[(int)generatorParameters[x].y];
			if(check(generatorParameters[x].z))
				newPos.z = paramRef.parameters[(int)generatorParameters[x].z];
			gameObjectRef = extraPoolRef.returnValidPart(
			                                                  (int) parameterArray[(int) generatorParameters[x].w]);
			if(gameObjectRef==null)
				gameObjectRef = (GameObject) GameObject.CreatePrimitive(PrimitiveType.Cube);
			else
	        	gameObjectRef = (GameObject) GameObject.Instantiate(gameObjectRef, transform.position, Quaternion.identity);
			gameObjectRef.transform.position = newPos + transform.position;
			gameObjectRef.transform.parent = transform;
		}
		//Destroy (GetComponent<extraPartGenerator>());
	}

	bool check(float param)
	{
		if ((int)param == -999)
			return false;
		else
			return true;
	}
}