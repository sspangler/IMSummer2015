using UnityEngine;
using System.Collections;

public class awakeRotate : MonoBehaviour {

	public Vector3 rotate;

	// Use this for initialization
	void Awake () {
		transform.Rotate (rotate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
