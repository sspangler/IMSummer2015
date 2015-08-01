using UnityEngine;
using System.Collections;

public class skater : MonoBehaviour {

	public float forwardspeed;
	public float speed;
	public GameObject head;
	public float score;
	public float scoreMultiplier;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//move forward
		transform.Translate(Vector3.forward * forwardspeed * Time.deltaTime);
		score += Time.deltaTime * scoreMultiplier;
		
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
		
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Kill") {
			transform.position = startPos;
		}
	}
	
}