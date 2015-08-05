﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class iceMove : MonoBehaviour {
	
	public float forwardSpeed;
	public float defaultSpeed = 25f;
	public float speed;
	public GameObject head;
	public float bodyLength;
	Vector3 startPos;
	public float lastRayDistance = 0f;
	public float score;
	public Text scoreText;
	//public GameObject lanePos;
	public skaterCamera camRef;
	
	// Use this for initialization
	void Awake () {
		camRef = GameObject.Find ("Ortho Camera").GetComponent<skaterCamera> ();
		startPos = transform.position;
		forwardSpeed = defaultSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		//move forward

		score += Time.deltaTime;
		scoreText.text = "Score:" + ((int) score).ToString ();

		transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
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
			lastRayDistance = hit.distance;
		}
		else lastRayDistance = bodyLength + 1f;
		
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left * speed * Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right * speed * Time.deltaTime);
		}
		//lanePos.transform.position = new Vector3(0f, lanePos.transform.position.y, lanePos.transform.position.z);
		camRef.moveToPosition (transform.position.z);
		if (transform.position.y < -30f)
			die ();
	}
	
	void OnTriggerEnter (Collider col) {
		if (col.tag == "Kill") {
			die ();
		}
	}

	public float returnDifficulty()
	{
		return forwardSpeed - defaultSpeed;
	}

	public void die()
	{
		enabled = false;
		forwardSpeed = 0f;
		if(GameObject.Find("runSceneScript"))
			GameObject.Find ("runSceneScript").GetComponent<runSceneScript>().show();
	}

	public void reset()
	{
		transform.position = startPos;
		forwardSpeed = defaultSpeed;
		enabled = true;
		score = 0f;
	}
}