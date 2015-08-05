using UnityEngine;
using System.Collections;

public class skaterCamera : MonoBehaviour {

	public float perspective;
	public Vector3 translation;
	public Vector2 extraRotation;
	public bool editor = true;
	public GameObject skaterRef;
	Vector3 origin;
	Camera cameraRef;
	float[] dataArray = {51f, 32f, 6f, 7f, -70f, -6f};
	public float[] incrementArray;

	// Use this for initialization
	void Awake () {
		origin = GameObject.Find ("Skater").transform.position;
		cameraRef = GetComponent<Camera> ();
		applyData ();
		storeData ();
		if (skaterRef == null)
			skaterRef = GameObject.Find ("Skater");
		transform.eulerAngles = new Vector3(-extraRotation.y, extraRotation.x, 0f);
		cameraRef.fieldOfView = perspective;
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
			transform.eulerAngles = new Vector3(-extraRotation.y, extraRotation.x, 0f);
			cameraRef.fieldOfView = perspective;
		}
	}

	public void moveToPosition(float skaterZ)
	{
		Vector3 lanePosition = new Vector3 (origin.x, origin.y, skaterZ);
		transform.position = lanePosition + translation;
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