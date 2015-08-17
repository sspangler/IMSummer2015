using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickUpCarrier : MonoBehaviour {

	public iceMove iceMove;
	public Slider invincibilitySlider;
	public bool hasInvincibility;
	public float invincibility;


	// Use this for initialization
	void Start () {
		iceMove = transform.parent.GetComponent<iceMove> ();
		invincibilitySlider.value = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Return) && invincibility > 0) {
			iceMove.killable = false;
			invincibility -= 40f * Time.deltaTime;
			invincibilitySlider.value = invincibility / 100;
		}
		if (Input.GetKeyUp (KeyCode.Return) || invincibility <= 0) {
			iceMove.killable = true;
		}

	}

	void OnTriggerEnter (Collider col) {
		if (col.name == "Invincibility(Clone)") {
			Destroy(col.gameObject);
			hasInvincibility = true;
			invincibility = 100f;
			invincibilitySlider.value = invincibility;
		}

	}
}