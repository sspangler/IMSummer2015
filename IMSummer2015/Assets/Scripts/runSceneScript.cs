using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class runSceneScript : MonoBehaviour {

	public GameObject overlay;
	public Text scoreText;
	public float score;

	// Use this for initialization
	void Awake () {
		hide ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void show()
	{
		score = GameObject.Find ("Skater").GetComponent<iceMove> ().score;
		scoreText.text = score.ToString ();
		overlay.SetActive (true);
	}

	public void hide()
	{
		overlay.SetActive (false);
	}

	public void playAgain()
	{
		hide ();
		GameObject.Find ("Skater").GetComponent<iceMove> ().reset ();
	}
}