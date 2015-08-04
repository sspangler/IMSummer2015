using UnityEngine;
using System.Collections;

public class zLog : MonoBehaviour {

	float lastZ;

	// Use this for initialization
	void Awake () {
		lastZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		float currentZ = transform.position.z;
		Debug.Log (name + ": " + ((currentZ - lastZ) / Time.deltaTime).ToString ());
		lastZ = currentZ;
	}
}
