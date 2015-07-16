using UnityEngine;
using System.Collections;

public class trackEndTeleporter : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag=="Player")
		{
			Vector3 difference = other.transform.position - transform.parent.GetComponent<trackPartData>().attachPoint.transform.position;
			other.transform.position = difference + GameObject.Find ("InitPart").GetComponent<trackPartData>().attachPoint.transform.position;
			GameObject.Find("TrackManager").GetComponent<lapTimer>().lap();
		}
	}
}
