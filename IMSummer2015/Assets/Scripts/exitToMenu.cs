using UnityEngine;
using System.Collections;

public class exitToMenu : MonoBehaviour {

	float doubleTimer = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(doubleTimer>0f)
				GameObject.Find("GameManager").GetComponent<loader>().loadScene(1, true, true);
			else doubleTimer = 0.3f;
		}
	}

	void OnGUI ()
	{
		GUI.TextArea(new Rect(0,0,139,25), "Back to Menu: 2xEsc"); 
	}
}
