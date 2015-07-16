using UnityEngine;
using System.Collections;

public class spriteCamera : MonoBehaviour {

	public GameObject theCamera;

	// Use this for initialization
	void Awake () {
		theCamera = GameObject.Find("Orth Camera");
		faceCamera ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void faceCamera()
	{
		Debug.Log ("1");
		if(theCamera==null)
		{
			if(GameObject.Find("Orth Camera"))
			{
				Debug.Log("Found Camera");
				theCamera = GameObject.Find("Orth Camera");
				faceCamera();
			}
			else Debug.LogWarning("Couldnt find orth camera");
		}
		else 
		{
			transform.LookAt (-theCamera.transform.up + transform.position);
		}
	}
}
