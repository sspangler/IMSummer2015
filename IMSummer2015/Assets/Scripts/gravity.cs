using UnityEngine;
using System.Collections;

public class gravity : MonoBehaviour {

	public float weight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Rigidbody> ().AddForce (0f, -weight * Time.fixedDeltaTime, 0f);
	}
}
