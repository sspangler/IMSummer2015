using UnityEngine;
using System.Collections;

public class sceneInit2 : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GameObject.Find ("GameManager").GetComponent<loader> ().loadScene (1);
		GameObject.Find ("GameManager").GetComponent<loader> ().changeTimer (300);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown)
			GameObject.Find ("GameManager").GetComponent<loader> ().loadScene (1, true);
	}
}
