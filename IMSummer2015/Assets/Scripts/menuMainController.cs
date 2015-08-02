using UnityEngine;
using System.Collections;

public class menuMainController : MonoBehaviour {

	loader loaderRef;

	// Use this for initialization
	void Awake () {
		loaderRef = GameObject.Find ("GameManager").GetComponent<loader> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void command(string a)
	{
		if (a == "run")
			loaderRef.loadScene (2);
		else if (a == "challenges")
			loaderRef.loadScene (3);
	}
}
