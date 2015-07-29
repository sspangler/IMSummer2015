using UnityEngine;
using System.Collections;

public class destroyer : MonoBehaviour {

	Transform playerRef;
	float drawDistance = 200f;

	// Use this for initialization
	void Awake () {
		playerRef = GameObject.Find ("Skater").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerRef.position.z > transform.position.z + drawDistance)
			GameObject.Destroy (gameObject, 0f);
	}
}
