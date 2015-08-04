using UnityEngine;
using System.Collections;

public class menuMainController : MonoBehaviour {

	loader loaderRef;
	GameObject managerRef;

	// Use this for initialization
	void Awake () {
		managerRef = GameObject.Find ("GameManager");
		loaderRef = managerRef.GetComponent<loader> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void command(string a)
	{
		if (a == "run")
		{
			loaderRef.loadScene (2);
			managerRef.GetComponent<challengeData>().challenge = false;
		}
		else if (a == "challenges")
		{
			loaderRef.loadScene (3);
			managerRef.GetComponent<challengeData>().challenge = true;
		}
	}
}
