using UnityEngine;
using System.Collections;

public class sceneInit : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<loader> ().loadScene (0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
