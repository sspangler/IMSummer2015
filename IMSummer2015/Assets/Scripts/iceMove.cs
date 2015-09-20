using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class iceMove : MonoBehaviour {
	
	public float forwardSpeed;
	public float defaultSpeed = 25f;
	public float turnSpeed;
	float currentTurnSpeed;
	public float turnAccel = 0.15f;
	float desiredTurnSpeed;
	float turnAccelNo;
	float decel = 1.5f;
	public GameObject head;
	public float bodyLength;
	Vector3 startPos;
	float xStart;
	public float lastRayDistance = 0f;
	public float score;
	public float scoreMultiplier;
	public float speedMultiplier;
	public Text scoreText;
	//public GameObject lanePos;
	public GameObject camGameObjectRef;
	public skaterCamera camRef;
	public bool killable;
	public LayerMask layers;

	float edgeDistance = 11.9f;
	
	// Use this for initialization
	void Awake () {
		killable = true;
		if(GameObject.Find ("Ortho Camera"))
			camRef = GameObject.Find ("Ortho Camera").GetComponent<skaterCamera> ();
		else
		{
			camRef = camGameObjectRef.GetComponent<skaterCamera>();
		}
		startPos = transform.position - GameObject.Find ("InitPart").transform.position;
		xStart = transform.position.x;
		forwardSpeed = defaultSpeed;
		currentTurnSpeed = 0f;
		scoreMultiplier = 1;
	}
	
	// Update is called once per frame
	void Update () {
		turnAccelNo = turnSpeed * turnAccel;
		//move forward
		score += Time.deltaTime;
		if(scoreText!=null)
			scoreText.text = "Score:" + ((int) score).ToString ();

		transform.Translate(Vector3.forward * forwardSpeed * speedMultiplier * Time.deltaTime);
		RaycastHit hit;
		Ray ray = new Ray(head.transform.position, -Vector3.up);

		//if something is under the player
		if (Physics.Raycast(ray, out hit, 9999f, layers))
		{
			// Move up if im hitting a platform
			if(hit.distance<=bodyLength)
			{
				transform.position += new Vector3(0f, bodyLength - hit.distance, 0f);
				lastRayDistance = hit.distance;
			}
			else
			{
				lastRayDistance = bodyLength + Time.deltaTime;
			}
		}
		else lastRayDistance = bodyLength + Time.deltaTime;

		float stepHeight = 0.5f;
		float duckHeight = 0.5f;
		float span = bodyLength - stepHeight - duckHeight;
		int rays = 3;
		float rayspan = span / ((rays*1.0f) - 1f);

		float a;
		float rayLength = 50f;
		float halfBody = 0.75f;
		float maxRight = (xStart+11.4f);
		float maxLeft = (xStart-11.4f);
		
		// Check right side
		for(int x=0;x<rays;x++)
		{
			a = returnRayLength(transform.position + new Vector3(0f, 
			                     bodyLength*0.5f - (duckHeight + (x*rayspan)), 0f), new Vector3(1f,0f,0f), rayLength);
			a += transform.position.x - halfBody;
			if(a<maxRight)
				maxRight = a;
		}
		
		// Check left side
		for(int x=0;x<rays;x++)
		{
			a = returnRayLength(transform.position + new Vector3(0f, 
			                     bodyLength*0.5f - (duckHeight + (x*rayspan)), 0f), new Vector3(-1f,0f,0f), rayLength);
			a = transform.position.x - a + halfBody;
			if(a>maxLeft)
				maxLeft = a;
		}

		// Get turning input
		desiredTurnSpeed = 0f;
		if (Input.GetKey (KeyCode.A)) {
			desiredTurnSpeed = -turnSpeed;
		}
		else if (Input.GetKey (KeyCode.D)) {
			desiredTurnSpeed = turnSpeed;
		}

		if(desiredTurnSpeed==0f)
		{
			if(Mathf.Abs(currentTurnSpeed)<(turnAccelNo*decel))
				currentTurnSpeed = 0f;
			else currentTurnSpeed = Mathf.Sign(currentTurnSpeed) * (Mathf.Abs(currentTurnSpeed) - (turnAccelNo*decel));
		}
		else
		{
			if(currentTurnSpeed==0f)
				currentTurnSpeed = Mathf.Sign(desiredTurnSpeed) * 0.0001f;
			if(Mathf.Sign(desiredTurnSpeed)!=Mathf.Sign(currentTurnSpeed))
			{
				currentTurnSpeed = Mathf.Sign (currentTurnSpeed) * (Mathf.Abs(currentTurnSpeed) - (turnAccelNo * decel));
				if(Mathf.Abs(currentTurnSpeed)>turnSpeed)
					currentTurnSpeed = Mathf.Sign (currentTurnSpeed) * turnSpeed;
			}
			else
			{
				currentTurnSpeed = Mathf.Sign (currentTurnSpeed) * (Mathf.Abs(currentTurnSpeed) + turnAccelNo);
				if(Mathf.Abs(currentTurnSpeed)>turnSpeed)
					currentTurnSpeed = Mathf.Sign (currentTurnSpeed) * turnSpeed;
			}
		}
		
		//Apply turning
		transform.Translate (((Vector3.right * currentTurnSpeed)) * Time.deltaTime);
		if(transform.position.x<maxLeft) // Check left limit
		{
			Vector3 pos = transform.position;
			pos.x = maxLeft;
			transform.position = pos;
		}
		if(transform.position.x>maxRight) // Check right limit
		{
			Vector3 pos = transform.position;
			pos.x = maxRight;
			transform.position = pos;
		}

		//lanePos.transform.position = new Vector3(0f, lanePos.transform.position.y, lanePos.transform.position.z);
		camRef.moveToPosition (transform.position.z);
		if (transform.position.y < -20f)
			die ();
	}
	
	void OnTriggerEnter (Collider col) {
		Debug.Log ("Enter" + " " + col.gameObject.name);
		if (col.gameObject.tag == "Death" && killable) {
			Debug.Log ("Dying");
			die ();
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.gameObject.tag == "Death" && killable) {
			Debug.Log ("Dying");
			die ();
		}
	}

	public float returnDifficulty()
	{
		return forwardSpeed - defaultSpeed;
	}

	public void die()
	{
		// Duplicate model, make duplicate do death anim
		// here
	
		if(Application.loadedLevelName=="TrackEditor")
		{
			reset();
			return;
		}

		enabled = false;
		forwardSpeed = 0f;
		if (GameObject.Find ("TrackManager").GetComponent<difficultyStack> ())
			GameObject.Find ("TrackManager").GetComponent<difficultyStack> ().reset ();
		if(GameObject.Find("runSceneScript"))
			GameObject.Find ("runSceneScript").GetComponent<runSceneScript>().show();
	}

	public void reset()
	{
		moveToStart ();
		forwardSpeed = defaultSpeed;
		enabled = true;
		score = 0f;
		GameObject[] obj = (GameObject[]) GameObject.FindObjectsOfType(typeof(GameObject));
		for (int x=0; x<obj.Length; x++)
		{
			if(obj[x].GetComponent<destroyer>())
				Destroy(obj[x], 0f);
		}
		if(GameObject.Find ("TrackManager"))
		{
			GameObject.Find ("TrackManager").GetComponent<trackGenerator>().nextSegmentPosition = GameObject.Find ("InitPart").GetComponent<trackPartData> ().endPoint.transform.position;
		}
	}

	public void moveToStart()
	{
		transform.position = startPos + GameObject.Find ("InitPart").transform.position;
	}

	public float returnRayLength(Vector3 position, Vector3 direction, float length = 9999f)
	{
		RaycastHit hit;
		Ray ray = new Ray(position, direction);

		//if something under the player
		if (Physics.Raycast(ray, out hit, length, layers))
		{
			Debug.Log(hit.collider.gameObject.name);
			return hit.distance;
		}
		return length;
	}
}