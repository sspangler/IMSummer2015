using UnityEngine;
using System.Collections;

public class plotter : MonoBehaviour {

	public string button;
	public float printLapse;
	float currentLapse;
	public float printTime;
	float currentPrintTime;

	// Use this for initialization
	void Awake () {
		currentPrintTime = 0f;
		currentLapse = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (button))
			currentPrintTime = printTime;

		if(currentPrintTime>0f)
		{
			currentLapse += Time.deltaTime;
			if(currentLapse>printLapse)
			{
				printBall();

				currentLapse += -printLapse;
				currentPrintTime += -printLapse;
			}
		}
	}

	void printBall()
	{
		GameObject.Instantiate (GameObject.CreatePrimitive (PrimitiveType.Sphere), transform.position, Quaternion.identity);
	}
}
