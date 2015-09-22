using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

	public float velocity;
	public float xGoal = 84f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * velocity * Time.deltaTime;
		if (transform.position.x >= xGoal)
			Debug.Break ();
	}
}
