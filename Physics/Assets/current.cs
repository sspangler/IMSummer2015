using UnityEngine;
using System.Collections;

public class current : MonoBehaviour {

	public float force;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (0f, -force * Time.deltaTime, 0f);
	}
}
