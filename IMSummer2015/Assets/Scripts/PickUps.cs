using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {

	iceMove skater;
	public bool speedBoost;
	float speedCounter;
	bool multiplier;
	float multiplierCounter;
	float defaultSpeedCounter = 1f;

	// Use this for initialization
	void Start () {
		skater = transform.parent.GetComponent<iceMove> ();
		skater.speedMultiplier = 1f;
		speedCounter = defaultSpeedCounter;
	}
	
	// Update is called once per frame
	void Update () {
		if (speedBoost) {
			speedCounter -= Time.deltaTime;
			if (speedCounter <= 0) {
				speedBoost = false;
				skater.speedMultiplier = 1;
				speedCounter = defaultSpeedCounter;
			}
		}

		if (multiplier) {
			multiplierCounter -= Time.deltaTime;
			if (multiplierCounter <= 0)
				skater.scoreMultiplier = 1f;
		}
	}

	void OnTriggerEnter (Collider col) {
		if (col.name == "Points(Clone)") {
			skater.score += 25 * skater.scoreMultiplier;
			Destroy(col.gameObject);
		}

		if (col.name == "SpeedBoost(Clone)") {
			skater.speedMultiplier += .20f;
			speedBoost = true;
			Destroy(col.gameObject);
		}

		if (col.name == "ScorePowerUp(Clone)") {
			skater.scoreMultiplier += .5f;
			Destroy(col.gameObject);
		}
	}

}