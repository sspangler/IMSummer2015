using UnityEngine;
using System.Collections;

public class lapTimer : MonoBehaviour {

	public float[] lapTimes;
	public bool timing = false;
	float timer = 0f;
	int maxLaps = 1000;

	// Use this for initialization
	void Awake () {
		lapTimes = new float[0];
	}
	
	// Update is called once per frame
	void Update () {
		if (timing)
			timer += Time.deltaTime;
		else timer = 0f;
	}

	public void startTiming()
	{
		timer = 0f;
		timing = true;
	}

	public void clearTimes()
	{
		lapTimes = new float[0];
	}

	public void stopTiming()
	{
		timer = 0f;
		timing = false;
	}

	public float lap()
	{
		if(lapTimes.Length>maxLaps)
			lapTimes = new float[0];

		float newLapTime = timer;
		timer = 0f;

		var oldLapTimes = lapTimes;
		lapTimes = new float[oldLapTimes.Length + 1];
		for (int x=0; x<oldLapTimes.Length; x++)
			lapTimes [x] = oldLapTimes [x];
		lapTimes [oldLapTimes.Length] = newLapTime;
		return newLapTime;
	}
}
