using UnityEngine;
using System.Collections;

public class BackgroundSpawner : MonoBehaviour {

	public GameObject[] farObjects;
	public GameObject[] midObjects;
	public GameObject[] closeObjects;
	public Vector3 farLanePos;
	public Vector3 middleLanePos;
	public Vector3 closeLanePos;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			SpawnFarObject();
		}

		for (int i = 0; i < 15; i++) {
			SpawnMiddleObject();
		}

		for (int i = 0; i < 25; i++) {
			SpawnCloseObject();
		}
	}

	public void SpawnFarObject () {
		int num1 = Random.Range(0,farObjects.Length);
		Instantiate(farObjects[num1], farLanePos, farObjects[num1].transform.rotation);
		farLanePos.z += 150f;
	}

	public void SpawnMiddleObject () {
		int num2 = Random.Range(0,midObjects.Length);
		Instantiate(midObjects[num2], middleLanePos, midObjects[num2].transform.rotation);
		middleLanePos.z += 125f;
	}

	public void SpawnCloseObject () {
		int num3 = Random.Range(0,closeObjects.Length);
		Instantiate(closeObjects[num3], closeLanePos, closeObjects[num3].transform.rotation);
		closeLanePos.z += 100;
	}
}
