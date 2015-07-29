using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class iceMove : MonoBehaviour {
	
	public float forwardspeed;
	public float speed;
	public GameObject head;
	public float bodyLength;
	Vector3 startPos;
	[HideInInspector]
	public float score;
	public Text scoreText;
	
	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//move forward
		transform.Translate(Vector3.forward * forwardspeed * Time.deltaTime);
		score += Time.deltaTime;
		scoreText.text = "Score:" + (int)score;
		RaycastHit hit;
		Ray ray = new Ray(head.transform.position, -Vector3.up);

		// Bit shift the index of the layer (8) to get a bit mask
		int layerMask = 1 << 8;
		
		// This would cast rays only against colliders in layer 8.
		// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;

		bool hitObject = false;
		//if something under the player
		if (Physics.Raycast(ray, out hit, 9999f, layerMask)) 
		{
			// Move up if im hitting a platform
			if(hit.distance<bodyLength)
			{
				transform.position += new Vector3(0f, bodyLength - hit.distance, 0f);
				hitObject = true;
			}
			else
			{
				hitObject = false;
			}
			// Fall (implement grabity) if there is no platform close enough
			Debug.Log (hit.distance);
		}

		if(!hitObject)
		{
			// apply some gravity.
			// placeholder gravity
			transform.position += new Vector3(0f, -1f * Time.deltaTime, 0f);
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