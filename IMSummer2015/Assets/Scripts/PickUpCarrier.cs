using UnityEngine;
using System.Collections;

public class PickUpCarrier : MonoBehaviour {

	public iceMove iceMove;
	public bool hasInvincibility;
	public float invincibility;


	// Use this for initialization
	void Start () {
		iceMove = transform.parent.GetComponent<iceMove> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return) && invincibility > 0) {
			iceMove.killable = false;
			invincibility -= 5 * Time.deltaTime;
		}
		if (Input.GetKeyUp (KeyCode.Return)) {
			iceMove.killable = true;
		}

	}

	void OnTriggerEnter (Collider col) {
		if (col.name == "Invincibility(Clone)") {
			Destroy(col.gameObject);
			hasInvincibility = true;
			invincibility = 100f;
		}

	}
}