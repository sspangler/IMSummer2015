using UnityEngine;
using System.Collections;

public class BackgroundSpawner : MonoBehaviour {

	public GameObject[] Objects;
	public Vector3 nextPos;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			SpawnObject();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpawnObject () {
		int num1 = Random.Range(0,Objects.Length);
		Instantiate(Objects[num1], nextPos, Objects[num1].transform.rotation);
		nextPos.z += 150f;
	}
}
