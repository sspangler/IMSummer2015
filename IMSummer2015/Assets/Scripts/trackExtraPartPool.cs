using UnityEngine;
using System.Collections;

public class trackExtraPartPool : MonoBehaviour {
	
	public string partName = "extraPart";
	public GameObject[] pool;
	public int[] numberPool;
	public GameObject endPart;
	int[] unusedParts = new int[]{-1};
	//public int[] parameterCount;
	
	// Use this for initialization
	void Awake () {
		//parameterCount = new int[0];
		for(int x=0;x<9999;x++)
		{
			if(Resources.Load ("Track Parts/" + partName + x.ToString()) && !unusedPartsContains(x))
			{
				Debug.Log ("Found TrackPiece #" + x.ToString());
				addprefab((GameObject) Resources.Load ("Track Parts/" + partName + x.ToString()), x);
			}
		}
		endPart = (GameObject)Resources.Load ("Track Parts/" + partName + 10000.ToString ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public int returnNewPartNumber(int currentNumber, int indexDifference)
	{
		currentNumber = Mathf.Abs (currentNumber);
		int index = 0;
		for(int x=0;x<numberPool.Length;x++)
		{
			if(currentNumber==numberPool[x])
				index = x;
		}
		
		while(true)
		{
			if(indexDifference<0)
			{
				if(index==0)
					index = numberPool.Length-1;
				else
					index += -1;
				indexDifference += 1;
			}
			else if(indexDifference>0)
			{
				if(index==numberPool.Length-1)
					index = 0;
				else
					index += 1;
				indexDifference += -1;
			}
			else break;
		}
		
		return numberPool [index];
	}
	
	void addprefab(GameObject toadd, int index)
	{
		var oldpool = pool;
		pool = new GameObject[oldpool.Length + 1];
		for(int x=0;x<oldpool.Length;x++)
		{
			pool[x] = oldpool[x];
		}
		pool[oldpool.Length] = toadd;
		
		var oldNumberPool = numberPool;
		numberPool = new int[oldNumberPool.Length + 1];
		for (int x=0; x<oldNumberPool.Length; x++)
			numberPool [x] = oldNumberPool [x];
		numberPool [oldNumberPool.Length] = index;
		
		/*var oldParameterCount = parameterCount;
		parameterCount = new int[parameterCount.Length + 1];
		for (int x=0; x<oldParameterCount.Length; x++)
			parameterCount [x] = oldParameterCount [x];
		int objectParameterCount = 0;
		if (toadd.GetComponent<extraParams> ())
			objectParameterCount = toadd.GetComponent<extraParams> ().parameters.Length;
		
		parameterCount [oldParameterCount.Length] = objectParameterCount;*/
	}
	
	public GameObject returnPart(int partNumber)
	{
		partNumber = Mathf.Abs (partNumber);
		for(int x=0;x<numberPool.Length;x++)
		{
			if(numberPool[x]==partNumber)
				return pool[x];
		}
		return pool [0];
	}
	
	public bool unusedPartsContains(int number)
	{
		for(int x=0;x<unusedParts.Length;x++)
		{
			if(unusedParts[x]==number)
				return true;
		}
		return false;
	}
	
	/*public int returnParameterCount(int partNo)
	{
		for(int x=0;x<numberPool.Length;x++)
		{
			if(partNo==numberPool[x])
				return parameterCount[x];
		}
		return 0;
	}*/
}