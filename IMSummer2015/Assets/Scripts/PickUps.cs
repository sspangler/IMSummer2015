using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {

	skater skater;
	bool speedBoost;
	float startSpeed;
	bool multiplier;
	float multiplierCounter;
	// Use this for initialization
	void Start () {
		startSpeed = skater.speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (speedBoost && startSpeed < skater.speed) {
			startSpeed -= Time.deltaTime;
			if (startSpeed >= skater.speed)
				speedBoost = false;
		}

		if (multiplier && skater.scoreMultiplier != 1f) {
			multiplierCounter -= Time.deltaTime;
			if (multiplierCounter <= 0)
				skater.scoreMultiplier = 1f;
		}
	}


	void OnTriggerEnter (Collider col) {
		if (col.tag == "Points") {
			skater.score += 25 * skater.scoreMultiplier;
		}

		if (col.tag == "Speed Boost") {
			skater.speed = 20f;
		}

		if (col.tag == "Point Boost") {
			skater.scoreMultiplier = 1.5f;
		}
	}
}
