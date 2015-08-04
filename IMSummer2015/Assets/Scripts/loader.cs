using UnityEngine;
using System.Collections;

public class loader : MonoBehaviour {

	public string[] scenes;
	int timer = 0;
	string command = "";
	int argumentA, argumentB;
	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (gameObject);
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

	public bool loadScene(int sceneIndex, bool overide = false)
	{
		if(command=="" || overide)
		{
			command = "loadScene";
			timer = 2;
			argumentA = sceneIndex;
			return false;
		}
		else
			return false;
	}

	public void changeTimer(int newTimer)
	{
		timer = newTimer;
	}
}