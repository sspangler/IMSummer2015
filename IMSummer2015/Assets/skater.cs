using UnityEngine;
using System.Collections;

public class skater : MonoBehaviour {

	public float forwardspeed;
	public float speed;
	public GameObject head;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//move forward
		transform.Translate(Vector3.forward * forwardspeed * Time.deltaTime);
		RaycastHit hit;
		Ray ray = new Ray(head.transform.position, -Vector3.up);

		//if something under the player
		if (Physics.Raycast(ray, out hit)) 
		{

		}
		
		
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