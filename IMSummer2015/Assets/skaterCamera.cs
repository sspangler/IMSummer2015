using UnityEngine;
using System.Collections;

public class skaterCamera : MonoBehaviour {

	public float perspective;
	public Vector3 translation;
	public Vector2 extraRotation;
	public bool editor = true;
	Vector3 origin;
	Camera cameraRef;
	float[] dataArray = {54f, 31f, 5.5f, 19f, -90f, 0f};
	public float[] incrementArray;

	// Use this for initialization
	void Awake () {
		origin = GameObject.Find ("Skater").transform.position;
		cameraRef = GetComponent<Camera> ();
		applyData ();
		storeData ();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject skaterRef = GameObject.Find ("Skater");
		if(editor)
		{
			changeValues(0, "7", "y");
			changeValues(1, "8", "u");
			changeValues(2, "9", "i");
			changeValues(3, "0", "o");
			changeValues(4, "-", "p");
			changeValues(5, "=", "[");
			applyData();
		}
		cameraRef.fieldOfView = perspective;

		Vector3 lanePosition = new Vector3 (origin.x, origin.y, skaterRef.transform.position.z);
		transform.position = lanePosition + translation;

		//transform.LookAt(lanePosition);
		//transform.Rotate(new Vector3(-extraRotation.y*perspective*16f/18f, extraRotation.x*perspective, 0f), Space.World);
		transform.eulerAngles = new Vector3(-extraRotation.y, extraRotation.x, 0f);
	}

	void storeData()
	{
		dataArray = new float[6];
		dataArray [0] = perspective;
		dataArray [1] = translation.x;
		dataArray [2] = translation.y;
		dataArray [3] = translation.z;
		dataArray [4] = extraRotation.x;
		dataArray [5] = extraRotation.y;
	}

	void applyData()
	{
		perspective = dataArray[0];
		translation.x = dataArray[1];
		translation.y = dataArray[2];
		translation.z = dataArray[3];
		extraRotation.x = dataArray[4];
		extraRotation.y = dataArray[5];
	}

	void changeValues(int index, string button1, string button2)
	{
		if (Input.GetKeyDown (button1))
			dataArray [index] += incrementArray [index];
		else if (Input.GetKeyDown (button2))
			dataArray [index] += -incrementArray [index];
	}

	public string dataToString()
	{
		string a = "Settings: (";
		for(int x=0;x<dataArray.Length;x++)
		{
			a += (dataArray[x]-dataArray[x]%0.01f).ToString();
			if(x<dataArray.Length-1)
				a += ", ";
		}
		return a + ")";
	}
}