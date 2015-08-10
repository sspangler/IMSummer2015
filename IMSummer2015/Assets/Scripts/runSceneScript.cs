using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class runSceneScript : MonoBehaviour {

	public GameObject overlay;
	public Text scoreText;
	public float score;
	public GameObject skaterRef;
	public GameObject managerRef;
	skaterCamera camRef;
	float deadTime = 2.5f;
	float deadPlayerZ;
	float spawnPlayerZ;
	float timer = 0f;

	// New variables for controlling camera

	// Use this for initialization
	void Awake () {
		camRef = GameObject.Find ("Ortho Camera").GetComponent<skaterCamera> ();
		hide ();
	}
	
	// Update is called once per frame
	void Update () {
		if(timer>0f)
		{
			timer += -Time.deltaTime;
			if(timer<=0f)
			{
				timer = 0f;
				camRef.moveToPosition(spawnPlayerZ);
				playAgain();
			}
			else camRef.moveToPosition(((spawnPlayerZ-deadPlayerZ)*((deadTime-timer)/deadTime))+deadPlayerZ);
		}
	}

	public void show()
	{
		/*
		 * Old show function()
		 * {
		score = GameObject.Find ("Skater").GetComponent<iceMove> ().score;
		scoreText.text = score.ToString ();
		overlay.SetActive (true);
		}
		*/
		trackGenerator genRef = GameObject.Find ("TrackManager").GetComponent<trackGenerator> ();
		Vector3 relativePosition = GameObject.Find ("InitPart").GetComponent<trackPartData> ().attachPoint.transform.localPosition;

		GameObject.Find ("InitPart").transform.position = genRef.nextSegmentPosition - relativePosition;
		deadPlayerZ = skaterRef.transform.position.z;
		skaterRef.GetComponent<iceMove> ().moveToStart ();
		spawnPlayerZ = skaterRef.transform.position.z;
		timer = deadTime;

		// Turn off trackGenerator
		managerRef.SetActive (false);
	}

	public void playAgain()
	{
		hide ();
		managerRef.SetActive (true);
		GameObject.Find ("Skater").GetComponent<iceMove> ().reset ();
	}

	public void hide()
	{
		overlay.SetActive (false);
	}
}