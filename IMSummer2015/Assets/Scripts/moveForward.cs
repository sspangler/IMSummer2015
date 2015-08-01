using UnityEngine;
using System.Collections;

public class moveForward : MonoBehaviour {

	public float speed;
	public float floatSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(0f,0f,speed*Time.deltaTime);

		transform.position += new Vector3(Input.GetAxis("Horizontal")*floatSpeed*Time.deltaTime,
		                                  Input.GetAxis("Vertical")*floatSpeed*Time.deltaTime, 0f);
	}
}
