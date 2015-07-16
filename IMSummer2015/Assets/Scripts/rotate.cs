using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public Vector3 force;

	// Use this for initialization
	void Awake () {
		GetComponent<Rigidbody> ().AddRelativeTorque (force);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
