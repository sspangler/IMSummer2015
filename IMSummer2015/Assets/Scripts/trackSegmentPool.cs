using UnityEngine;
using System.Collections;

public class trackSegmentPool : MonoBehaviour {

	public class segment
	{
		public string name;
		public int[] parts;
		public Vector4[] parameters;
		public Vector3 difficulty;

		public segment(string newName, int[] newParts, float[] newParameters, Vector3 newDifficulty)
		{
			name = newName;
			parts = newParts;
			parameters = floatToVectorArray(newParameters);
			difficulty = newDifficulty;
		}

		public segment(string newName, int[] newParts, Vector4[] newParameters, Vector3 newDifficulty)
		{
			name = newName;
			parts = newParts;
			parameters = newParameters;
			difficulty = newDifficulty;
		}

		public segment()
		{
			name = "";
			parts = new int[0];
			parameters = new Vector4[0];
			difficulty = new Vector3(0f,0f,0f);
		}

		public Vector4[] floatToVectorArray(float[] a)
		{
			Vector4[] array = new Vector4[a.Length / 4];
			for(int x=0;x<array.Length;x++)
			{
				array[x] = new Vector4(a[(x*4)+0], a[(x*4)+1], a[(x*4)+2], a[(x*4)+3]);
			}
			return array;
		}

		public float[] vectorToFloatArray(Vector4[] a)
		{
			float[] array = new float[a.Length * 4];
			for(int x=0;x<a.Length;x++)
			{
				array[(x*4)+0] = a[x].x;
				array[(x*4)+1] = a[x].y;
				array[(x*4)+2] = a[x].z;
				array[(x*4)+3] = a[x].w;
			}
			return array;
		}
	}

	public segment[] segmentPool;
	public bool loaded = false;
	public int maxLoad = 100000;
	public int currentLoad = 0;
	public float loadWeight = 100;
	bool skipOneFrame = false;
	float targetDeltaTime;
	usave_file usaveRef;

	// Use this for initialization
	void Awake () {
		targetDeltaTime = 1f / 40f;
		usaveRef = GetComponent<usave_file> ();
		segmentPool = new segment[0];
	}
	
	// Update is called once per frame
	void Update () {
		if(!loaded)
		{
			loadSegments((int)loadWeight);
			if(skipOneFrame)
			{
				float dt = Time.deltaTime;
				if(dt>targetDeltaTime*1.15f)
					loadWeight = loadWeight * (targetDeltaTime / dt);
				else if(dt<=targetDeltaTime*1.01f)
					loadWeight = ((loadWeight+10f) * 1.1f);
				if(loadWeight<10f)
					loadWeight = 10f;
			}
			skipOneFrame = true;
		}
	}

	void OnGUI()
	{
		if(!loaded)
			GUI.TextArea(new Rect(0,0,37,25), ((int) (100f*currentLoad/maxLoad)).ToString()+"%"); 
	}

	public void loadSegments(int count = -1)
	{
		if (count == -1)
			count = maxLoad;
		if (loaded)
			return;
		int nextMaxLoad = currentLoad + count;
		if (nextMaxLoad > maxLoad)
		{
			nextMaxLoad = maxLoad;
			loaded = true;
		}

		for(int x=currentLoad;x<nextMaxLoad;x++)
		{
			if(usaveRef.ifSlot(x))
			{
				load(x);
			}
		}
		currentLoad = nextMaxLoad;
	}

	void load(int index)
	{
		Debug.Log ("Loading " + index);
		// Load file
		usaveRef.slot = index;
		usaveRef.allResize (0);
		usaveRef.loadFile (true);

		// Create segment
		segment newSegment = new segment (usaveRef.sarray[0], usaveRef.iarray, usaveRef.farray, usaveRef.varray[0]);

		// Insert segment into array
		var oldArray = segmentPool;
		segment[] newArray;
		newArray = new segment[oldArray.Length + 1];
		for(int x=0;x<oldArray.Length;x++)
			newArray[x] = oldArray[x];
		newArray [oldArray.Length] = newSegment;
		segmentPool = newArray;
	}
}