using UnityEngine;
using System.Collections;

public class loader : MonoBehaviour {

	public string[] scenes;
	int timer = 0;
	string command = "";
	int argumentA, argumentB;
	// Use this for initialization
	void Awake () {
		Application.targetFrameRate = 60;
		argumentA = -1;
		argumentB = -1;
	}
	
	// Update is called once per frame
	void Update () {

		if(timer==0)
		{
			if(command=="loadScene")
			{
				Application.LoadLevel(scenes[argumentA]);
				command = "";
			}
		}

		if(timer>0)
			timer += -1;
	}

	public bool loadScene(int sceneIndex)
	{
		if(command=="")
		{
			command = "loadScene";
			timer = 3;
			argumentA = sceneIndex;
			return false;
		}
		else
			return false;
	}
}