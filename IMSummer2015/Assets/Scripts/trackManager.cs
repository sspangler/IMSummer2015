using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class trackManager : MonoBehaviour {

	public bool racing = false;
	public GameObject racingCamera;
	public GameObject editingCamera;
	public int currentPart = 0;
	public int[] partNumbers;
	public GameObject[] trackParts;
	public Vector3 cameraDirection;
	public float currentCameraDistance = 100f;
	public Vector4[] partParameters; // (Part Index, Part No, which parameter, value)
	Vector3 hoverPlanePosition;
	Vector3 hoverPlaneEulers;
	GameObject hoverPlaneRef;
	public Text scoreText;
	float zoomSpeed = 100f;
	int selectedExtraParam = 0;
	int selectedPartParams = 0;
	bool editor = true;
	public Vector4[] paramReferences; // {(min, max, inc, reference (partParameters index))}

	public string partName = "NameText";
	public int partMinDifficulty = 0;
	public int partMaxDifficulty = 1;
	public string partSegmentNumber = "0";
	usave_file usaveRef;

	// Use this for initialization
	void Awake () {
		if (GetComponent<usave_file> ())
			usaveRef = GetComponent<usave_file> ();
		partParameters = new Vector4[0];
		paramReferences = new Vector4[0];
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
			string editorCameraSettings = "";
			if(editor)
				editorCameraSettings = racingCamera.GetComponent<skaterCamera>().dataToString();
			string[] racingArray = new string[]{"A/D - Strafe",
				"R - Reset to start",
				"T - Switch between racer/editor",
				"Camera Settings: '7/Y', '8/U', '9,I',",
				"'0/O', '-/P', '=/['",
				"Camera Settings: 'Angle',",
				"'translate x,y,z', 'rotation x,y'",
				editorCameraSettings};

			var lapTimes = GetComponent<lapTimer>().lapTimes;
			for(int x=0;x<lapTimes.Length;x++)
			{
			}

			GUITextList(12, Color.black,  racingArray);
		}
		else
		{
			// Draw segment buttons
			/* Save new 32 78 , 42 83
			 * Save as 32 84 , 42 89
			 * Name 43 78 , 57 83
			 * No 43 84 , 57 89
			 * Load 58 78 , 68 83
			 * Next 58 84 , 68 87
			 * Prev 58 88 , 68 91
			 */
			Rect a;
			a = rectGroup(1, 32, 78, 42, 83)[0];
			if(GUI.Button(a, "Save New"))
			{
				int saveNo = 0;
				trackSegmentPool.segment s = new trackSegmentPool.segment();
				while(true)
				{
					saveNo = Random.Range(0, 1000000);
					if(!usaveRef.ifSlot(saveNo))
						break;
				}
				partSegmentNumber = saveNo.ToString();
				usaveRef.slot = saveNo;
				usaveRef.allResize(0);
				usaveRef.iarray = partNumbers;
				usaveRef.farray = s.vectorToFloatArray(partParameters);
				usaveRef.varray = new Vector3[1];
				usaveRef.varray[0] = new Vector3((float) partMinDifficulty, (float) partMaxDifficulty, 0f);
				usaveRef.sarray = new string[1];
				usaveRef.sarray[0] = partName;
				usaveRef.saveFile();
			}
			a = rectGroup(1, 32, 84, 42, 89)[0];
			if(GUI.Button(a, "Save as#"))
			{
				int saveNo = int.Parse(partSegmentNumber);
				trackSegmentPool.segment s = new trackSegmentPool.segment();
				if(saveNo>=0 && saveNo < 1000000)
				{
					usaveRef.slot = saveNo;
					usaveRef.allResize(0);
					usaveRef.iarray = partNumbers;
					usaveRef.farray = s.vectorToFloatArray(partParameters);
					usaveRef.varray = new Vector3[1];
					usaveRef.varray[0] = new Vector3((float) partMinDifficulty, (float) partMaxDifficulty, 0f);
					usaveRef.sarray = new string[1];
					usaveRef.sarray[0] = partName;
					usaveRef.saveFile();
				}
			}
			a = rectGroup(1, 43, 78, 57, 83)[0];
			partName = GUI.TextField(a, partName);
			a = rectGroup(1, 43, 84, 57, 89)[0];
			partSegmentNumber = GUI.TextField(a, partSegmentNumber);
			a = rectGroup(1, 58, 78, 68, 83)[0];
			if(GUI.Button(a, "Load"))
			{
				int saveNo = int.Parse(partSegmentNumber);
				load(saveNo);
			}
			a = rectGroup(1, 58, 84, 68, 87)[0];
			if(GUI.Button(a, "Next"))
			{
				int saveNo = int.Parse(partSegmentNumber) + 1;
				bool foundSegment = false;
				for(int x=saveNo;x<1000000;x++)
				{
					if(usaveRef.ifSlot(x))
					{
						load (x);
						foundSegment = true;
						break;
					}
				}
				if(!foundSegment)
				{
					for(int x=0;x<saveNo;x++)
					{
						if(usaveRef.ifSlot(x))
						{
							load (x);
							foundSegment = true;
							break;
						}
					}
				}
			}
			a = rectGroup(1, 58, 88, 68, 91)[0];
			if(GUI.Button(a, "Prev"))
			{
				int saveNo = int.Parse(partSegmentNumber) - 1;
				bool foundSegment = false;
				for(int x=saveNo;x>=0;x--)
				{
					if(usaveRef.ifSlot(x))
					{
						load (x);
						foundSegment = true;
						break;
					}
				}
				if(!foundSegment)
				{
					for(int x=1000000;x>saveNo;x--)
					{
						if(usaveRef.ifSlot(x))
						{
							load (x);
							foundSegment = true;
							break;
						}
					}
				}
			}
			/* Min+ 36 71 , 41 74
			 * Min- 36 74 , 41 77
			 * Max+ 59 71 , 64 74
			 * Max- 59 74 , 64 77
			 */
			a = rectGroup(1, 36, 71, 41, 74)[0];
			if(GUI.Button(a, "Min+"))
			{
				if(partMinDifficulty+1<partMaxDifficulty)
					partMinDifficulty += 1;
			}
			a = rectGroup(1, 36, 74, 41, 77)[0];
			if(GUI.Button(a, "Min-"))
			{
				if(partMinDifficulty>0)
					partMinDifficulty += -1;
			}
			a = rectGroup(1, 59, 71, 64, 74)[0];
			if(GUI.Button(a, "Max+"))
			{
				partMaxDifficulty += 1;
			}
			a = rectGroup(1, 59, 74, 64, 77)[0];
			if(GUI.Button(a, "Max-"))
			{
				if(partMaxDifficulty>(partMinDifficulty+1))
					partMaxDifficulty += -1;
			}
			GUI.Label(rectGroup(1, 42, 68, 46, 71)[0], partMinDifficulty.ToString());
			GUI.Label(rectGroup(1, 56, 68, 58, 71)[0], partMaxDifficulty.ToString());
			hoverPlaneRef.GetComponent<iceMove>().forwardSpeed = hoverPlaneRef.GetComponent<iceMove>().defaultSpeed +
				GUI.HorizontalSlider(rectGroup(1, 45, 72, 55, 76)[0], hoverPlaneRef.GetComponent<iceMove>().forwardSpeed - hoverPlaneRef.GetComponent<iceMove>().defaultSpeed, partMinDifficulty, partMaxDifficulty);
			GUI.Label(rectGroup(1, 49, 68, 51, 71)[0], hoverPlaneRef.GetComponent<iceMove>().returnDifficulty().ToString());
			// Draw parameter sliders
			if(selectedPartParams>0)
			{
				Rect[] rectArray = rectGroup(selectedPartParams, 80, 10, 96, 90, 20);
				float oldValue;
				bool redoTrack = false;
				for(int x=0;x<rectArray.Length;x++)
				{
					oldValue = partParameters[(int)paramReferences[x].w].w;
					partParameters[(int)paramReferences[x].w].w = GUI.HorizontalSlider(rectArray[x], partParameters[(int)paramReferences[x].w].w, paramReferences[x].x, paramReferences[x].y);
					if(!redoTrack)
					{
						if(oldValue!=partParameters[(int)paramReferences[x].w].w)
							redoTrack = true;
						selectedExtraParam = x;
					}
				}
				if(redoTrack)
					generateTrack();
			}

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
				//scoreText.text = "";
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
					resetParameterSelect();
				}
				else if(Input.GetKeyDown (KeyCode.D)) // Previous piece
				{
					if(currentPart>0)
					{
						trackParts[currentPart].GetComponent<trackPartData>().setHighlight(false);
						currentPart += -1;
					}
					resetParameterSelect();
				}
				else if(Input.GetKeyDown (KeyCode.W)) // Change piece, up
				{
					changePart(1);
					resetParameterSelect();
					generateTrack();
				}
				else if(Input.GetKeyDown (KeyCode.S)) // Change piece, down
				{
					changePart(-1);
					resetParameterSelect();
					generateTrack();
				}
				else if(Input.GetKeyDown (KeyCode.Backspace))
				{
					subtractPart(currentPart);
					resetParameterSelect();
				}
				else if(Input.GetKeyDown (KeyCode.F))
				{
					partNumbers[currentPart] = -partNumbers[currentPart];
					generateTrack();
				}
				else if(Input.GetKeyDown (KeyCode.DownArrow))
				{
					selectedExtraParam += 1;
					if(selectedExtraParam>=selectedPartParams)
						selectedExtraParam = 0;
				}
				else if(Input.GetKeyDown (KeyCode.UpArrow))
				{
					selectedExtraParam += -1;
					if(selectedExtraParam<0)
						selectedExtraParam = selectedPartParams - 1;
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

		int parametersLeft = 0;
		for(int x=0;x<trackParts.Length;x++)
		{
			parametersLeft = 0;
			if(trackParts[x].GetComponent<extraParams>())
				parametersLeft = trackParts[x].GetComponent<extraParams>().parameters.Length;
			if(parametersLeft==0)
				continue;
			for(int y=0;y<partParameters.Length;y++)
			{
				if((int)partParameters[y].x==x)
				{
					trackParts[x].GetComponent<extraParams>().parameters[(int)partParameters[y].z] = partParameters[y].w;
					parametersLeft += -1;
					if(parametersLeft==0)
						break;
				}
			}
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
			newParam = new Vector4(index, partNo, x, GetComponent<trackPartPool>().returnPart(partNo).GetComponent<extraParams>().properties[x].x);
			insertParameter(newParam);
		}
	}

	void subParameters(int partIndex, bool collapseNumbers = false)
	{
		for(int x=partParameters.Length-1;x>=0;x--)
		{
			if((int) partParameters[x].x==partIndex)
			{
				removeParameter(x);
			}
		}

		if(collapseNumbers)
		{
			for(int x=partParameters.Length-1;x>=0;x--)
			{
				if(partParameters[x].x>partIndex)
					partParameters[x].x += -1;
			}
		}
	}

	void insertParameter(Vector4 newParam)
	{
		Vector4[] oldParameters = partParameters;
		partParameters = new Vector4[oldParameters.Length+1];

		for (int x=0; x<oldParameters.Length; x++)
			partParameters [x] = oldParameters [x];

		partParameters [partParameters.Length - 1] = newParam;
	}

	void removeParameter(int index)
	{
		Vector4[] oldParameters = partParameters;
		partParameters = new Vector4[oldParameters.Length-1];

		for(int x=0;x<index;x++)
		{
			partParameters[x] = oldParameters[x];
		}
		for(int x=index;x<partParameters.Length;x++)
		{
			partParameters[x] = oldParameters[x+1];
		}
	}

	void resetParameterSelect()
	{
		selectedExtraParam = 0;
		selectedPartParams = 0;
		updateParamReferences();
	}
	
	Vector3 returnProperties(int partNo, int paramNo)
	{
		return
			GameObject.Find("TrackManager").GetComponent<trackPartPool>().returnPart(partNo).GetComponent<extraParams>().properties[paramNo];
	}

	int countCurrentParameters(int partIndex)
	{
		int count = 0;
		for(int x=0;x<partParameters.Length;x++)
		{
			if((int) partParameters[x].x==partIndex)
				count += 1;
		}
		
		return count;
	}

	void updateParamReferences()
	{
		selectedPartParams = countCurrentParameters (currentPart);
		paramReferences = new Vector4[selectedPartParams];
		int valuesLeft = paramReferences.Length;
		Vector3 currentProperties;

		if (valuesLeft == 0)
			return;

		// Get properties and references
		for(int x=0;x<partParameters.Length;x++)
		{
			if((int) partParameters[x].x==currentPart)
			{
				currentProperties = returnProperties((int) partParameters[x].y, (int) partParameters[x].z);
				paramReferences[(int) partParameters[x].z] = new Vector4(currentProperties.x, currentProperties.y, currentProperties.z,
				                                                         (float) x);
				valuesLeft += -1;
				if(valuesLeft==0)
					break;
			}
		}
	}

	public Rect x100Rect (float x, float y, float sizex, float sizey, int pixelSpacer=0)
	{
		return x100Rect((int)Mathf.Round(x), (int)Mathf.Round(y), (int)Mathf.Round (sizex), (int)Mathf.Round(sizey), pixelSpacer);
	}
	
	public Rect x100Rect (int x, int y, int sizex, int sizey = -1, int pixelSpacer=0)
	{
		return new Rect(x*Mathf.Round(Screen.width/100f), 
		                y*Mathf.Round(Screen.height/100f), 
		                sizex*Mathf.Round(Screen.width/100f),
		                sizey*Mathf.Round(Screen.height/100f)-pixelSpacer);
	}
	
	public Rect[] rectGroup(int count, int x, int y, int x2, int y2, int pixelSpacer = 10)
	{
		float boxHeight = y2 - y;
		float buttonHeight;
		if(count>1)
			buttonHeight = boxHeight/(count-1);
		else 
		{
			buttonHeight = boxHeight;
			pixelSpacer = 0;
		}
		Rect[] returnGroup;
		returnGroup = new Rect[count];
		for(int a=0;a<count;a++)
		{
			returnGroup[a] = x100Rect(x,y+buttonHeight*a,
			                          x2-x,buttonHeight, pixelSpacer);
		}
		return returnGroup;
	}

	void load(int saveNo)
	{
		if(saveNo>=0 && saveNo < 1000000)
		{
			trackSegmentPool.segment s = new trackSegmentPool.segment();
			usaveRef.slot = saveNo;
			usaveRef.allResize(0);
			usaveRef.loadFile(true);
			partNumbers = usaveRef.iarray;
			partParameters = s.floatToVectorArray(usaveRef.farray);
			partMinDifficulty = (int) usaveRef.varray[0].x;
			partMaxDifficulty = (int) usaveRef.varray[0].y;
			partName = usaveRef.sarray[0];
			partSegmentNumber = saveNo.ToString();
		}
	}
}