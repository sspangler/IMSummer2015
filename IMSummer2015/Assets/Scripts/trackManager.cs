using UnityEngine;
using System.Collections;

public class trackManager : MonoBehaviour {

	public bool racing = false;
	public GameObject racingCamera;
	public GameObject editingCamera;
	public int currentPart = 0;
	public int[] partNumbers;
	public GameObject[] trackParts;
	public Vector3 cameraDirection;
	public float currentCameraDistance = 100f;
	public Vector3[] partParameters; // (Part Index, Part No, which parameter)
	Vector3 hoverPlanePosition;
	Vector3 hoverPlaneEulers;
	GameObject hoverPlaneRef;
	float zoomSpeed = 100f;

	// Use this for initialization
	void Awake () {
		partParameters = new Vector3[0];
		hoverPlaneRef = GameObject.Find ("Skater");
		hoverPlaneRef.SetActive (false);
		partNumbers = new int[1];
		trackParts = new GameObject[1];
		partNumbers [0] = 0;
		generateTrack ();
		setCamera (false);
		currentPart = 0;
		hoverPlanePosition = hoverPlaneRef.transform.position;
		hoverPlaneEulers = hoverPlaneRef.transform.eulerAngles;
	}

	void OnGUI()
	{
		if(racing)
		{
			string[] racingArray = new string[]{"A/D - Strafe",
				"Mouse1 - gas",
				"R - Reset to start",
				"T - Switch between racer/editor"};

			var lapTimes = GetComponent<lapTimer>().lapTimes;
			for(int x=0;x<lapTimes.Length;x++)
			{
			}

			GUITextList(12, Color.black,  racingArray);
		}
		else
		{
			GUITextList(12, Color.black,  new string[]{"Current(" + GetComponent<trackPartPool>().returnNewPartNumber(partNumbers[currentPart], 0) + "): " + trackParts[currentPart].GetComponent<trackPartData>().name,
										"W - Change part",
			                            "S - Change part",
			                            "A - Nav Next part",
			                            "D - Nav Previous part",
			                            "T - Switch between racer/editor",
			                            "F - Invert part",
										"Backspace - Delete current part"});
		}
	}

	// Update is called once per frame
	void Update () {
		updateEditorCamera ();

		if(racing)
		{
			if(Input.GetKeyDown (KeyCode.T))
			{
				resetHoverplaneTransform();
				hoverPlaneRef.SetActive(false);
				racing = false;
				GetComponent<lapTimer>().stopTiming();
			}
			else
			{
				if(Input.GetKeyDown(KeyCode.R))
					resetHoverplaneTransform();
				/*else if(Input.GetKeyDown (KeyCode.Comma))
				{
					hoverPlaneRef.GetComponent<rotationControls>().strafer = 
						!hoverPlaneRef.GetComponent<rotationControls>().strafer;
				}*/
			}
		}
		else if(!racing)
		{
			if(Input.GetKeyDown (KeyCode.T))
			{
				generateTrack(true);
				hoverPlaneRef.SetActive(true);
				trackParts[currentPart].GetComponent<trackPartData>().setHighlight(false);
				racing = true;
				GetComponent<lapTimer>().clearTimes();
				GetComponent<lapTimer>().startTiming();
			}
			else
			{
				currentCameraDistance += -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
				if(currentCameraDistance<10f)
					currentCameraDistance = 10f;
				else if(currentCameraDistance>700f)
					currentCameraDistance = 700f;

				if(Input.GetKeyDown (KeyCode.A)) // Next piece
				{
					trackParts[currentPart].GetComponent<trackPartData>().setHighlight(false);
					if(currentPart<(trackParts.Length-1))
					{
						currentPart += 1;
					}
					else
					{
						addPart();
					}

				}
				else if(Input.GetKeyDown (KeyCode.D)) // Previous piece
				{
					if(currentPart>0)
					{
						trackParts[currentPart].GetComponent<trackPartData>().setHighlight(false);
						currentPart += -1;
					}
				}
				else if(Input.GetKeyDown (KeyCode.W)) // Change piece, up
				{
					changePart(1);

					generateTrack();
				}
				else if(Input.GetKeyDown (KeyCode.S)) // Change piece, down
				{
					changePart(-1);

					generateTrack();
				}
				else if(Input.GetKeyDown (KeyCode.Backspace))
				{
					subtractPart(currentPart);
				}
				else if(Input.GetKeyDown (KeyCode.F))
				{
					partNumbers[currentPart] = -partNumbers[currentPart];
					generateTrack();
				}
				/*else if(Input.GetKeyDown (KeyCode.Comma))
				{
					hoverPlaneRef.GetComponent<rotationControls>().strafer = 
						!hoverPlaneRef.GetComponent<rotationControls>().strafer;
				}*/
			}
		}
	}

	void setCamera()
	{
		setCamera (racing);
	}

	void setCamera(bool isRacing)
	{
		racing = isRacing;
		racingCamera.SetActive (racing);
		editingCamera.SetActive (!racing);
	}
	
	public float returnCameraDistance()
	{
		return 100f;
		//return trackParts [currentPart].GetComponent<trackPartData> ().cameraDistance;
	}

	public void addPart()
	{
		var oldPartNumbers = partNumbers;
		partNumbers = new int[oldPartNumbers.Length+1];
		for (int x=0; x<oldPartNumbers.Length; x++)
			partNumbers [x] = oldPartNumbers [x];
		partNumbers [oldPartNumbers.Length] = oldPartNumbers[oldPartNumbers.Length-1];
		currentPart = partNumbers.Length - 1;

		addParameters (currentPart);

		generateTrack ();
	}

	public bool subtractPart(int index)
	{
		if (partNumbers.Length < 2)
			return false;
		var oldPartNumbers = partNumbers;
		partNumbers = new int[oldPartNumbers.Length-1];
		for (int x=0; x<index; x++)
			partNumbers [x] = oldPartNumbers [x];
		for (int x=(index+1); x<oldPartNumbers.Length; x++)
			partNumbers [x - 1] = oldPartNumbers [x];

		currentPart = currentPart - 1;
		if(currentPart<0)
			currentPart = 0;

		subParameters (index, true);

		generateTrack ();

		return true;
	}

	public void updateEditorCamera()
	{
		setCamera ();
		if(racing)
		{

		}
		else
		{
			trackParts[currentPart].GetComponent<trackPartData>().setHighlight(true);
			//editingCamera.transform.position = trackParts[currentPart].transform.position +
			//									Vector3.Normalize(cameraDirection)*returnCameraDistance();
			editingCamera.transform.position = trackParts[currentPart].transform.position +
											Vector3.Normalize(cameraDirection)*currentCameraDistance;
			editingCamera.transform.LookAt(trackParts[currentPart].transform.position);
		}
	}

	public void generateTrack(bool withEndPart = false)
	{
		for (int x=0; x<trackParts.Length; x++)
			GameObject.Destroy (trackParts [x]);
		if(GameObject.Find("Part10000(Clone)"))
			GameObject.Destroy (GameObject.Find("Part10000(Clone)"));

		trackPartData currentData;
		GameObject currentObject;
		trackParts = new GameObject[partNumbers.Length];
		for(int x=0;x<trackParts.Length;x++)
		{
			trackParts[x] = (GameObject) GameObject.Instantiate(GetComponent<trackPartPool>().returnPart(partNumbers[x]), transform.position, Quaternion.identity);
			currentObject = trackParts[x];
			currentData = currentObject.GetComponent<trackPartData>();
			if(partNumbers[x]<0)
				currentData.invert();
			currentObject.transform.forward = lastForward(x);
			currentObject.transform.position = lastPosition(x) + (currentObject.transform.position - currentData.attachPoint.transform.position);
		}
		if(withEndPart)
		{
			GameObject a = (GameObject) GameObject.Instantiate(GetComponent<trackPartPool>().endPart, transform.position, Quaternion.identity);
			a.transform.forward = lastForward(trackParts.Length);
			a.transform.position = lastPosition(trackParts.Length) + (trackParts[trackParts.Length-1].transform.position - trackParts[trackParts.Length-1].GetComponent<trackPartData>().attachPoint.transform.position);
		}
	}

	public Vector3 lastPosition(int index)
	{
		if (index == 0)
			return GameObject.Find ("InitPart").GetComponent<trackPartData> ().endPoint.transform.position;
		else
			return trackParts [index-1].GetComponent<trackPartData> ().endPoint.transform.position;
	}

	public Vector3 lastForward(int index)
	{
		if(index==0)
			return new Vector3(0f, 0f, 1f);
		else
			return trackParts[index-1].GetComponent<trackPartData>().nextForward;
	}

	public void resetHoverplaneTransform()
	{
		hoverPlaneRef.transform.position = hoverPlanePosition;
		hoverPlaneRef.transform.eulerAngles = hoverPlaneEulers;
		//hoverPlaneRef.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		//hoverPlaneRef.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
	}

	void changePart(int navigateInt)
	{
		int maintainInversion = 1;
		if(partNumbers[currentPart]!=0)
			maintainInversion = partNumbers[currentPart]/Mathf.Abs(partNumbers[currentPart]);
		partNumbers[currentPart] = GetComponent<trackPartPool>().returnNewPartNumber(
			partNumbers[currentPart], navigateInt);
		partNumbers[currentPart] = partNumbers[currentPart] * maintainInversion;

		subParameters(currentPart);
		addParameters(currentPart);
	}

	void GUITextList (int textSize, Color textColor, string[] strArray)
	{
		GUIStyle listStyle = GUIStyle.none;
		listStyle.fontSize = textSize;
		listStyle.normal.textColor = textColor;
		string temp = "";
		for (int i = 0; i < strArray.Length; i++) 
		{
			temp += strArray[i];
			temp += '\n';
		}
		GUI.Label(new Rect(0,0,0,0), temp , listStyle);
	}

	void addParameters(int index)
	{
		Vector3 newParam;
		int partIndex = index;
		int partNo = partNumbers [index];
		int parameterCount = GetComponent<trackPartPool> ().returnParameterCount(partNo);
		// parameter = (partindex, partno, value)
		for(int x=0;x<parameterCount;x++)
		{
			newParam = new Vector3(index, partNo, GetComponent<trackPartPool>().pool[partNo].GetComponent<extraParams>().properties[x].x);
			insertParameter(newParam);
		}
	}

	void subParameters(int partIndex, bool collapseNumbers = false)
	{
		for(int x=0;x<partParameters.Length;x++)
		{
			if(partParameters[x].x==partIndex)
			{
				removeParameter(x);
			}
		}

		if(collapseNumbers)
		{
			for(int x=0;x<partParameters.Length;x++)
			{
				if(partParameters[x].x>partIndex)
					partParameters[x].x += -1;
			}
		}
	}

	void insertParameter(Vector3 newParam)
	{
		Vector3[] oldParameters = partParameters;
		partParameters = new Vector3[oldParameters.Length+1];

		for (int x=0; x<oldParameters.Length; x++)
			partParameters [x] = oldParameters [x];

		partParameters [partParameters.Length - 1] = newParam;
	}

	void removeParameter(int index)
	{
		Vector3[] oldParameters = partParameters;
		partParameters = new Vector3[oldParameters.Length-1];

		for(int x=0;x<index;x++)
		{
			partParameters[x] = oldParameters[x];
		}
		for(int x=index;x<partParameters.Length;x++)
		{
			partParameters[x] = oldParameters[x+1];
		}
	}

	int countCurrentParameters(int partIndex)
	{
		for(int x=0;x<partParameters.Length;x++)
		{

		}
	}
}