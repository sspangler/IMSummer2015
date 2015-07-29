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
	usave_file usaveRef;

	// Use this for initialization
	void Awake () {
		usaveRef = GetComponent<usave_file> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadSegments()
	{
		if (loaded)
			return;
		loaded = true;

		for(int x=0;x<1000000;x++)
		{
			if(usaveRef.ifSlot(x))
			{
				load(x);
			}
		}
	}

	void load(int index)
	{
		// Load file
		usaveRef.slot = index;
		usaveRef.allResize (0);
		usaveRef.loadFile (true);

		// Create segment
		segment newSegment = new segment (usaveRef.sarray[0], usaveRef.iarray, usaveRef.farray, usaveRef.varray[0]);

		// Insert segment into array
		var oldArray = segmentPool;
		var newArray = new segment[oldArray.Length+1];
		for(int x=0;x<oldArray.Length;x++)
			newArray[x] = oldArray[x];
		newArray [oldArray.Length] = newSegment;
	}
}