using UnityEngine;
using System.Collections;

public class PickUps : MonoBehaviour {

	iceMove skater;
	bool speedBoost;
	float startSpeed;
	bool multiplier;
	float multiplierCounter;
	// Use this for initialization
	void Start () {
		skater = transform.parent.GetComponent<iceMove> ();
		startSpeed = skater.forwardSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (speedBoost) {
			startSpeed -= Time.deltaTime;
			if (startSpeed >= skater.speed)
				speedBoost = false;
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
			skater.speed += 10f;
			//speedBoost = true;
			Destroy(col.gameObject);

		}

		if (col.name == "ScorePowerUp(Clone)") {
			skater.scoreMultiplier += .5f;
			Destroy(col.gameObject);
		}
	}

}
