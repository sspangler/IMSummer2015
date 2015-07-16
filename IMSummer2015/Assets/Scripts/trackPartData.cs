using UnityEngine;
using System.Collections;

public class trackPartData : MonoBehaviour {

	public string name;
	public GameObject attachPoint;
	public GameObject endPoint;
	public Vector3 nextForward;
	//public float cameraDistance;
	public int[] canAttachTo; // What can go AFTER this piece
	public GameObject[] highlightObjects;
	public GameObject[] pieces;

	// Use this for initialization
	void Awake () {
		setHighlight (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setHighlight(bool enabled)
	{
		for(int x=0;x<highlightObjects.Length;x++)
			highlightObjects[x].SetActive(enabled);
	}

	public bool attachContains(int index)
	{
		for(int x=0;x<canAttachTo.Length;x++)
		{
			if(canAttachTo[x]==index)
				return true;
		}
		return false;
	}

	public void invert()
	{
		nextForward.x = -nextForward.x;
		transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}
}
