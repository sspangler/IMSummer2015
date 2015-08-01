using UnityEngine;
using System.Collections;

public class parallax : MonoBehaviour {

	public float parallaxScale;
	public Vector3 origin;
	public Vector3 playerOrigin;
	GameObject skaterRef;

	// Use this for initialization
	void Awake () {
		skaterRef = GameObject.Find ("Skater");
		origin = transform.position;
		playerOrigin = skaterRef.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = skaterRef.transform.position.z - playerOrigin.z;
		transform.position = origin + new Vector3 (0f, 0f, distance * parallaxScale);
	}

	public Vector3 goToOrigin()
	{
		transform.position = origin;
		return origin;
	}
}
