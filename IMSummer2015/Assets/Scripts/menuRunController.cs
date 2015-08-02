using UnityEngine;
using System.Collections;

public class menuRunController : MonoBehaviour {
	
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
		if (a == "back")
			loaderRef.loadScene (1);
		else if (a == "next")
		{
			loaderRef.loadScene (6);
		}
	}
}
