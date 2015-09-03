using UnityEngine;
using System.Collections;

public class trackGenerator : MonoBehaviour {

	public Vector3 nextSegmentPosition;
	public bool inEditor = false;
	public GameObject playerRef;
	public GameObject managerRef;
	public Vector4[] parameters;
	trackSegmentPool segmentPoolRef;
	trackPartPool partPoolRef;
	PickUpPool pickUpPoolRef;
	difficultyStack stackRef;
	BackgroundSpawner backgroundRef;
	int backgroundCounter = 0;

	// Use this for initialization
	void Awake () {
		stackRef = GetComponent<difficultyStack> ();
		if(!GetComponent<trackManager>())
		{
			managerRef = GameObject.Find ("GameManager");
			if(GetComponent<trackManager>())
			{
				if(GetComponent<trackManager>().enabled)
					inEditor = true;
			}
			if(playerRef==null)
				playerRef = GameObject.Find ("Skater");
			nextSegmentPosition = GameObject.Find ("InitPart").GetComponent<trackPartData> ().endPoint.transform.position;
			segmentPoolRef = managerRef.GetComponent<trackSegmentPool> ();
			partPoolRef = managerRef.GetComponent<trackPartPool> ();
			pickUpPoolRef = managerRef.GetComponent<PickUpPool>();
			backgroundRef = GetComponent<BackgroundSpawner>();
			segmentPoolRef.loadSegments ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(playerRef!=null)
		{	
			if(playerRef.transform.position.z>(nextSegmentPosition.z-90f))
				drawSegment();
		}
		else playerRef = GameObject.Find ("Skater");
	}

	public void drawSegment()
	{
		int difficultyClass = stackRef.pop ();
		float speed = playerRef.GetComponent<iceMove> ().returnDifficulty();
		int a;
		int segmentNumber = 0;
		Vector3 difficultyReference;
		while(true)
		{
			a = Random.Range(0, segmentPoolRef.segmentPool.Length);
			difficultyReference = segmentPoolRef.segmentPool[a].difficulty;
			if(difficultyReference.x<=speed && difficultyReference.y>=speed && 
			   segmentPoolRef.segmentPool[a].difficultyClass == difficultyClass)
			{
				segmentNumber = a;
				break;
			}
		}
		generateTrack (segmentPoolRef.segmentPool [a], false, true);
	}

	public void generateTrack(trackSegmentPool.segment seg, bool withEndPart = false, bool addDestroyer = false)
	{
		generateTrack (seg.parts, seg.parameters, withEndPart, addDestroyer);
	}

	public void generateTrack(int[] partNumbers, Vector4[] extraParams, bool withEndPart = false, bool addDestroyer = false)
	{
		//for (int x=0; x<trackParts.Length; x++)
		//	GameObject.Destroy (trackParts [x]);
		//if(GameObject.Find("Part10000(Clone)"))
		//	GameObject.Destroy (GameObject.Find("Part10000(Clone)"));
		trackPartData currentData;
		GameObject currentObject;
		GameObject[] trackParts = new GameObject[partNumbers.Length];
		for(int x=0;x<trackParts.Length;x++)
		{
			backgroundCounter += 1;
			if (backgroundCounter >=10) {
				backgroundRef.SpawnFarObject();
				backgroundCounter = 0;
			}
			trackParts[x] = (GameObject) GameObject.Instantiate(partPoolRef.returnPart(partNumbers[x]), transform.position, Quaternion.identity);
			currentObject = trackParts[x];
			currentData = currentObject.GetComponent<trackPartData>();
			if(partNumbers[x]<0)
				currentData.invert();
			currentObject.transform.forward = lastForward(x, trackParts);
			currentObject.transform.position = lastPosition(x, trackParts) + (currentObject.transform.position - currentData.attachPoint.transform.position);
		
			//pick up spawner
			int num = Random.Range(0,pickUpPoolRef.pickUps.Length * 2);
			if (num < pickUpPoolRef.pickUps.Length) {
				GameObject pickUp = (GameObject) Instantiate(pickUpPoolRef.pickUps[num], transform.position, Quaternion.identity);
				pickUp.transform.parent = currentObject.transform;
				pickUp.transform.position = lastPosition(x, trackParts) + (currentObject.transform.position - currentData.attachPoint.transform.position);
				pickUp.transform.position += Vector3.up * 3;
			}
		}

		int parametersLeft = 0;
		for(int x=0;x<trackParts.Length;x++)
		{
			parametersLeft = 0;
			if(trackParts[x].GetComponent<extraParams>())
				parametersLeft = trackParts[x].GetComponent<extraParams>().parameters.Length;
			if(parametersLeft==0)
				continue;
			for(int y=0;y<extraParams.Length;y++)
			{
				if((int)extraParams[y].x==x)
				{
					trackParts[x].GetComponent<extraParams>().parameters[(int)extraParams[y].z] = extraParams[y].w;
					parametersLeft += -1;
					if(parametersLeft==0)
						break;
				}
			}
		}

		if(addDestroyer)
		{
			for(int x=0;x<trackParts.Length;x++)
			{
				trackParts[x].AddComponent<destroyer>();
			}
		}

		if(!inEditor)
		{
			nextSegmentPosition = trackParts[trackParts.Length-1].GetComponent<trackPartData>().endPoint.transform.position;
		}

		if(withEndPart)
		{
			GameObject a = (GameObject) GameObject.Instantiate(GetComponent<trackPartPool>().endPart, transform.position, Quaternion.identity);
			a.transform.forward = lastForward(trackParts.Length, trackParts);
			a.transform.position = lastPosition(trackParts.Length, trackParts) + (trackParts[trackParts.Length-1].transform.position - trackParts[trackParts.Length-1].GetComponent<trackPartData>().attachPoint.transform.position);
		}
	}

	public Vector3 lastPosition(int index, GameObject[] trackParts)
	{
		if (index == 0 && inEditor)
			return GameObject.Find ("InitPart").GetComponent<trackPartData> ().endPoint.transform.position;
		else if (index == 0 && !inEditor)
			return nextSegmentPosition;
		else
			return trackParts [index-1].GetComponent<trackPartData> ().endPoint.transform.position;
	}
	
	public Vector3 lastForward(int index, GameObject[] trackParts)
	{
		if(index==0)
			return new Vector3(0f, 0f, 1f);
		else
			return trackParts[index-1].GetComponent<trackPartData>().nextForward;
	}
}
