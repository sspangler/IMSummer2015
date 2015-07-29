using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

// Developer: Scott Ohlrich
// Last Update: 5/13/15
// USave File System

public class usave_file : MonoBehaviour {

	[Header("Version Info:")]
	[Tooltip("This component's save file version.")]
	public bool isAndroidOrMobileIOS = false;
	[Tooltip("This component's save file version.")]
	public string version = "a"; // Stored in save for updating saves.
	[Tooltip("The version of the most recently loaded file.")]
	public string versionLoaded = "";
	[Header("Save File & Location Info:")]
	[Tooltip("The name of your save file.")]
	public string saveName = "save"; // The name of the save file.
	[Tooltip("The slot number appended to the end of the saveName.")]
	public int slot = 1; // Concatenated to saveName.
	// useRootLocation and saveLocation arent used yet
	[Tooltip("Should I save to the game's install root?")]
	public bool useRootLocation = true; // If false, you must specify where to save using saveLocation.
	[Tooltip("Where to save to. If useRootLocation is true, this can be left blank, or else it will append this location to the end of the root directory (Example: '\folder1' would save to the root + '\folder1').")]
	public string saveLocation = ""; // File is saved here, or here+root if(useRootLocation).
	[Header("File Protection Info:")]
	[Tooltip("Should I encrypt save files?")]
	public bool useEncryption = false;
	[Tooltip("The key used to encrypt save data. This can be any series of random characters, but should be at least 20 characters. The longer the key is, the more random the encryption.")]
	public string encryptionKey = "Encryption";
	[Tooltip("If this is on then a hash value will be added to the save file, generated via the cheatDetectionKey. Even if you do not use it, this can be left on and only has a minimal effect on save speed.")]
	public bool useCheatDetection = false;
	[Tooltip("If the last file loaded had an incorrect hash value (in other words, was modified by something besides your game) then this will be true.")]
	public bool cheatDetected = false; // Tells if the loaded file is modified.
	[Tooltip("The key used to generate the cheat detection hash value")]
	public string cheatDetectionKey = "Cheat Detection Key";
	[Header("New Save File Values:")]
	[Tooltip("The value that floats are initiliazed to when using newFile()")]
	public float fNull = 0f;
	[Tooltip("The value that ints are initiliazed to when using newFile()")]
	public int iNull = 0;
	[Tooltip("The value that bools are initiliazed to when using newFile()")]
	public bool bNull = false;
	[Tooltip("The value that Vector3s are initiliazed to when using newFile()")]
	public Vector3 vNull = Vector3.zero;
	[Tooltip("The value that strings are initiliazed to when using newFile()")]
	public string sNull = "";
	[Header("The public save data:")]
	public float[] farray;
	public int[] iarray;
	public bool[] barray;
	public Vector3[] varray;
	public string[] sarray;

	Char[] values = {'/', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ';', '|', ',', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '-', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '_'};

	int extensions = 0;
	
	/*
	 * Save order:
	 * 1. version
	 * 2. farray length
	 * 3. iarray length
	 * 4. barray length
	 * 5. varray length
	 * 6. sarray length
	 * 7. farray
	 * 8. iarray
	 * 9. barray
	 * 10. varray
	 * 11. sarray
	 * 12. encryption
	 */
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void WriteToFile(string Target, string Text)
	{
		//Debug.Log ("Converting");
		byte[] bArray = Encoding.ASCII.GetBytes (Text);
		//Debug.Log ("Writing");
		File.WriteAllBytes (Target, bArray);
		//Debug.Log ("Done Writing");
		//File.WriteAllText(Target, Text);
	}  
	
	string ReadFile(string Target)
	{
		//return File.ReadAllText(Target);
		if(File.Exists(Target))
		{
			return(Encoding.ASCII.GetString (File.ReadAllBytes(Target)));
		}
		else
			return("shit");
	}
	
	bool iffile(string Target)
	{
		if(File.Exists(Target))
			return true;
		else
			return false;
	}
	
	public bool ifSlot(int checkslot)
	{
		int tempslot = slot;
		bool exists;
		slot = checkslot;
		if(File.Exists(datapath() + concatenate()))
		{
			exists = true;
		}
		else
			exists = false;
		
		slot = tempslot;
		
		return exists;
	}
	
	public string datapath()
	{
		if(useRootLocation)
		{
			if(isAndroidOrMobileIOS)
				return Application.persistentDataPath + saveLocation + "/";
			else
				return Application.dataPath + saveLocation + "/";
		}
		else
			return saveLocation;
	}
	
	public string concatenate()
	{
		return saveName + slot.ToString();
	}
	
	float stringtofloat(string thestring)
	{
		float result;
		float.TryParse (thestring, out result);
		return result;
	}
	
	int stringtoint(string thestring)
	{
		int result;
		int.TryParse (thestring, out result);
		return result;
	}
	
	public void newFile()
	{
		versionLoaded = version;
		cheatDetected = false;
		for(int x=0;x<farray.Length;x++)
		{
			farray[x] = fNull;
		}

		for(int x=0;x<iarray.Length;x++)
		{
			iarray[x] = iNull;
		}
		
		for(int x=0;x<barray.Length;x++)
		{
			barray[x] = bNull;
		}
		
		for(int x=0;x<sarray.Length;x++)
		{
			sarray[x] = sNull;
		}
		
		for(int x=0;x<varray.Length;x++)
		{
			varray[x] = vNull;
		}
	}
	
	public string savestring()
	{
		if(checkForBadStrings())
		{
			Debug.LogError("Cannot save strings that contain ';' or '|'. These are used for formatting the save files.");
			return "";
		}
		string filetext = "";
		
		filetext += version + "|";
		filetext += farray.Length.ToString () + "|";
		filetext += iarray.Length.ToString () + "|";
		filetext += barray.Length.ToString () + "|";
		filetext += varray.Length.ToString () + "|";
		filetext += sarray.Length.ToString () + "|";
		
		for(int x=0;x<farray.Length;x++) // 1 Serialize float values
		{
			filetext += farray[x].ToString();
			if((x+1)<farray.Length)
			{
				filetext += ";";
			}
		}
		filetext += "|";
		// Serialization done
		
		for(int x=0;x<iarray.Length;x++) // 2 Serialize int values
		{
			filetext += iarray[x].ToString();
			if((x+1)<iarray.Length)
			{
				filetext += ";";
			}
		}
		filetext += "|";
		// Serialization done
		
		for(int x=0;x<barray.Length;x++) // 3 Serialize bool values
		{
			if(barray[x])
			{
				filetext += "1";
			}
			else
				filetext += "0";
			if((x+1)<barray.Length)
			{
				filetext += ";";
			}
		}
		filetext += "|";
		// Serialization done
		
		for(int x=0;x<varray.Length;x++) // 4 Serialize vector3 values
		{
			filetext += varray[x].x.ToString() + "," + varray[x].y.ToString() + "," + varray[x].z.ToString();
			if((x+1)<varray.Length)
			{
				filetext += ";";
			}
		}
		filetext += "|";
		// Serialization done
		
		for(int x=0;x<sarray.Length;x++) // 5 Serialize sarray values
		{
			filetext += sarray[x];
			if((x+1)<sarray.Length)
			{
				filetext += ";";
			}
		}
		filetext += "|";
		// Serialization done
		if(useCheatDetection)
		{
			int crypt = 0;
			int counter = 0;
			
			for(int x=0;x<filetext.Length;x++)
			{
				crypt += filetext[x] * cheatDetectionKey[counter];
				counter += 1;
				if(counter==cheatDetectionKey.Length)
				{
					counter = 0;
				}
			}
			filetext += crypt.ToString ();
		}
		else
			filetext += "_";

		if(useEncryption)
		{
			int counter = 0;
			Char[] array = new char[filetext.Length];
			for(int x=0;x<filetext.Length;x++)
			{
				array[x] = filetext[x];
			}
			for(int x=0;x<array.Length;x++)
			{
				if(returnint(array[x])!=-1)
				{
					array[x] = returnchar((returnint(array[x]) + encryptionKey[counter] * encryptionKey[counter+1]) % returnlength());
				}
				counter += 1;
				if(counter==(encryptionKey.Length-1))
				{
					counter = 0;
				}
			}
			filetext = "";
			for(int x=0;x<array.Length;x++)
			{
				filetext += array[x].ToString();
			}
		}
		
		return filetext;
	}
	
	public void saveFile()
	{
		savewithstring (savestring());
	}
	
	public void savewithstring(string filetext)
	{
		WriteToFile (datapath () + concatenate (), filetext);
	}

	public void deleteFile()
	{
		File.Delete (datapath () + concatenate ());
	}
	
	public string loadstring()
	{
		if(!ifSlot(slot))
		{
			return ""; // No file to load
		}
		return ReadFile (datapath () + concatenate ());
	}
	
	public void loadFile(bool useFileArraySizes = false)
	{
		loadfromstring (loadstring(), useFileArraySizes);
	}
	
	public bool loadfromstring(string filetext, bool useFileArraySizes = false)
	{
		newFile ();
		
		string file = filetext;
		
		if (file == "")
		{
			Debug.LogError ("Blank load string. This may occur because of other errors, check above.");
			return false;
		}
		
		if(useEncryption)
		{
			int counter = 0;
			Char[] array = new char[file.Length];
			
			for(int x=0;x<file.Length;x++)
			{
				array[x] = file[x];
			}
			
			for(int x=0;x<array.Length;x++)
			{
				if(returnint (array[x])!=-1)
				{
					array[x] = returnchar((returnint(array[x]) - encryptionKey[counter] * encryptionKey[counter+1]) % -returnlength());
				}
				counter += 1;
				if(counter==(encryptionKey.Length-1))
				{
					counter = 0;
				}
			}
			
			file = "";
			for(int x=0;x<array.Length;x++)
			{
				file += array[x].ToString();
			}
			Debug.Log (file);
		}
		var zones = file.Split ("|" [0]);
		if(zones.Length!=(12+extensions))
		{
			Debug.LogError("Save file not formatted correctly. Are you incorrectly loading an encrypted file? Are you trying to load a file as encrypted even though it is not encrypted?");
			return false;
		}
		if(useFileArraySizes)
		{
			resizeFIBVS(stringtoint(zones[1]), stringtoint(zones[2]), stringtoint(zones[3]), stringtoint(zones[4]), stringtoint(zones[5]));
			return loadfromstring(filetext);
		}
		string withoutkey = "";
		for(int x=0;x<11;x++)
		{
			withoutkey += zones[x] + "|";
		}
		if(useCheatDetection)
		{
			int crypt = 0;
			int counter = 0;
			
			for(int x=0;x<withoutkey.Length;x++)
			{
				crypt += withoutkey[x] * cheatDetectionKey[counter];
				counter += 1;
				if(counter==cheatDetectionKey.Length)
				{
					counter = 0;
				}
			}
			if(zones[11]==crypt.ToString())
			{
				cheatDetected = false;
			}
			else
			{
				cheatDetected = true;
				Debug.LogWarning("Cheat Detection" + " Key: " + zones[11] + " File's Key: " + crypt);
			}
		}
		
		versionLoaded = zones [0];
		
		// Start loading
		
		var filearray = zones[6].Split(";" [0]);
		for(int x=0;true;x++)
		{
			if((x<filearray.Length)&&(x<farray.Length))
			{
				farray[x] = stringtofloat(filearray[x]);
			}
			else if((x<filearray.Length)&&(x>=farray.Length)&&(filearray[x]!=""))
			{
				Debug.LogWarning ("Float array (farray) is not long enough to load all of this file.");
				break;
			}
			else
				break;
		}
		
		filearray = zones[7].Split(";" [0]);
		for(int x=0;true;x++)
		{
			if((x<filearray.Length)&&(x<iarray.Length))
			{
				iarray[x] = stringtoint(filearray[x]);
			}
			else if((x<filearray.Length)&&(x>=iarray.Length)&&(filearray[x]!=""))
			{
				Debug.LogWarning ("Integer array (iarray) is not long enough to load all of this file.");
				break;
			}
			else
				break;
		}
		
		filearray = zones[8].Split(";" [0]);
		for(int x=0;true;x++)
		{
			if((x<filearray.Length)&&(x<barray.Length))
			{
				if(filearray[x]=="1")
					barray[x] = true;
				else
					barray[x] = false;
			}
			else if((x<filearray.Length)&&(x>=barray.Length)&&(filearray[x]!=""))
			{
				Debug.LogWarning ("Bool array (barray) is not long enough to load all of this file.");
				break;
			}
			else
				break;
		}
		
		filearray = zones[9].Split(";" [0]);
		var vector3array = filearray[0].Split("," [0]);
		for(int x=0;true;x++)
		{
			if((x<filearray.Length)&&(x<varray.Length))
			{
				vector3array = filearray[x].Split ("," [0]);
				varray[x] = new Vector3(stringtofloat(vector3array[0]), stringtofloat(vector3array[1]), stringtofloat(vector3array[2]));
			}
			else if((x<filearray.Length)&&(x>=varray.Length)&&(filearray[x]!=""))
			{
				Debug.LogWarning ("Vector3 array (varray) is not long enough to load all of this file.");
				break;
			}
			else
				break;
		}
		
		filearray = zones[10].Split(";" [0]);
		for(int x=0;true;x++)
		{
			if((x<filearray.Length)&&(x<sarray.Length))
			{
				sarray[x] = filearray[x];
			}
			else if((x<filearray.Length)&&(x>=sarray.Length))
			{
				Debug.LogWarning ("String array (sarray) is not long enough to load all of this file.");
				break;
			}
			else
				break;
		}
		return true; // Loaded
	}
	
	int returnint(char character)
	{
		for(int x=0;x<values.Length;x++)
		{
			if(values[x]==character)
			{
				return x;
			}
		}
		
		return -1;
	}
	
	char returnchar(int number)
	{
		if(number<0)
			number += values.Length;
		
		if(number<values.Length)
			return values[number];
		else
			return '?';
	}
	
	int returnlength()
	{
		return values.Length;
	}
	
	public bool insertAt(int where, float[] data, bool autoResize = false)
	{
		if(autoResize)
		{
			if(where + data.Length > farray.Length)
				farrayResize(where + data.Length);
		}
		if(where>farray.Length)
			return false;
		for(int x=0;true;x++)
		{
			if((x+where<farray.Length)&&(x<data.Length))
			{
				farray[x+where] = data[x];
			}
			else if((x+where>=farray.Length)&&(x<data.Length))
			{
				return false;
			}
			else
				return true;
		}
	}
	
	public bool insertAt(int where, int[] data, bool autoResize = false)
	{
		if(autoResize)
		{
			if(where + data.Length > iarray.Length)
				iarrayResize(where + data.Length);
		}
		if(where>iarray.Length)
			return false;
		for(int x=0;true;x++)
		{
			if((x+where<iarray.Length)&&(x<data.Length))
			{
				iarray[x+where] = data[x];
			}
			else if((x+where>=iarray.Length)&&(x<data.Length))
			{
				return false;
			}
			else
				return true;
		}
	}
	
	public bool insertAt(int where, bool[] data, bool autoResize = false)
	{
		if(autoResize)
		{
			if(where + data.Length > barray.Length)
				barrayResize(where + data.Length);
		}
		if(where>barray.Length)
			return false;
		for(int x=0;true;x++)
		{
			if((x+where<barray.Length)&&(x<data.Length))
			{
				barray[x+where] = data[x];
			}
			else if((x+where>=barray.Length)&&(x<data.Length))
			{
				return false;
			}
			else
				return true;
		}
	}
	
	public bool insertAt(int where, Vector3[] data, bool autoResize = false)
	{
		if(autoResize)
		{
			if(where + data.Length > varray.Length)
				varrayResize(where + data.Length);
		}
		if(where>varray.Length)
			return false;
		for(int x=0;true;x++)
		{
			if((x+where<varray.Length)&&(x<data.Length))
			{
				varray[x+where] = data[x];
			}
			else if((x+where>=varray.Length)&&(x<data.Length))
			{
				return false;
			}
			else
				return true;
		}
	}
	
	public bool insertAt(int where, string[] data, bool autoResize = false)
	{
		if(autoResize)
		{
			if(where + data.Length > sarray.Length)
				sarrayResize(where + data.Length);
		}
		if(where>sarray.Length)
			return false;
		for(int x=0;true;x++)
		{
			if((x+where<sarray.Length)&&(x<data.Length))
			{
				sarray[x+where] = data[x];
			}
			else if((x+where>=sarray.Length)&&(x<data.Length))
			{
				return false;
			}
			else
				return true;
		}
	}
	
	public void insertBefore(int where, float[] data, bool autoResize = false)
	{
		insertAt (where + data.Length, farrayGet(where, farray.Length, true), autoResize);
		insertAt (where, data);
	}
	
	public void insertBefore(int where, int[] data, bool autoResize = false)
	{
		insertAt (where + data.Length, iarrayGet(where, iarray.Length, true), autoResize);
		insertAt (where, data);
	}
	
	public void insertBefore(int where, bool[] data, bool autoResize = false)
	{
		insertAt (where + data.Length, barrayGet(where, barray.Length, true), autoResize);
		insertAt (where, data);
	}
	
	public void insertBefore(int where, Vector3[] data, bool autoResize = false)
	{
		insertAt (where + data.Length, varrayGet(where, varray.Length, true), autoResize);
		insertAt (where, data);
	}
	
	public void insertBefore(int where, string[] data, bool autoResize = false)
	{
		insertAt (where + data.Length, sarrayGet(where, sarray.Length, true), autoResize);
		insertAt (where, data);
	}

	public void insertBefore(int where, float data, bool autoResize = false)
	{
		float[] newdata = new float[1];
		newdata [0] = data;
		insertBefore (where, newdata, autoResize);
	}

	public void insertBefore(int where, int data, bool autoResize = false)
	{
		int[] newdata = new int[1];
		newdata [0] = data;
		insertBefore (where, newdata, autoResize);
	}

	public void insertBefore(int where, bool data, bool autoResize = false)
	{
		bool[] newdata = new bool[1];
		newdata [0] = data;
		insertBefore (where, newdata, autoResize);
	}

	public void insertBefore(int where, Vector3 data, bool autoResize = false)
	{
		Vector3[] newdata = new Vector3[1];
		newdata [0] = data;
		insertBefore (where, newdata, autoResize);
	}

	public void insertBefore(int where, string data, bool autoResize = false)
	{
		string[] newdata = new string[1];
		newdata [0] = data;
		insertBefore (where, newdata, autoResize);
	}

	public void insertAt(int where, float data, bool autoResize = false)
	{
		float[] newdata = new float[1];
		newdata [0] = data;
		insertAt (where, newdata, autoResize);
	}
	
	public void insertAt(int where, int data, bool autoResize = false)
	{
		int[] newdata = new int[1];
		newdata [0] = data;
		insertAt (where, newdata, autoResize);
	}
	
	public void insertAt(int where, bool data, bool autoResize = false)
	{
		bool[] newdata = new bool[1];
		newdata [0] = data;
		insertAt (where, newdata, autoResize);
	}
	
	public void insertAt(int where, Vector3 data, bool autoResize = false)
	{
		Vector3[] newdata = new Vector3[1];
		newdata [0] = data;
		insertAt (where, newdata, autoResize);
	}
	
	public void insertAt(int where, string data, bool autoResize = false)
	{
		string[] newdata = new string[1];
		newdata [0] = data;
		insertAt (where, newdata, autoResize);
	}
	
	public float[] farrayGet(int where, int length, bool ignoreErrors = false)
	{
		if(length<1 && !ignoreErrors)
		{
			length = farray.Length;
			Debug.LogWarning("You just asked for an array of length < 1");
		}
		if(length+where>farray.Length)
			length = farray.Length - where;
		float[] array = new float[length];
		for(int x=0;x<length;x++)
		{
			if(x+where<farray.Length)
				array[x] = farray[where+x];
		}
		return array;
	}
	
	public int[] iarrayGet(int where, int length, bool ignoreErrors = false)
	{
		if(length<1 && !ignoreErrors)
		{
			length = iarray.Length;
			Debug.LogWarning("You just asked for an array of length < 1");
		}
		if(length+where>iarray.Length)
			length = iarray.Length - where;
		int[] array = new int[length];
		for(int x=0;x<length;x++)
		{
			if(x+where<iarray.Length)
				array[x] = iarray[where+x];
		}
		return array;
	}
	
	public bool[] barrayGet(int where, int length, bool ignoreErrors = false)
	{
		if(length<1 && !ignoreErrors)
		{
			length = barray.Length;
			Debug.LogWarning("You just asked for an array of length < 1");
		}
		if(length+where>barray.Length)
			length = barray.Length - where;
		bool[] array = new bool[length];
		for(int x=0;x<length;x++)
		{
			if(x+where<barray.Length)
				array[x] = barray[where+x];
		}
		return array;
	}
	
	public Vector3[] varrayGet(int where, int length, bool ignoreErrors = false)
	{
		if(length<1 && !ignoreErrors)
		{
			length = varray.Length;
			Debug.LogWarning("You just asked for an array of length < 1");
		}
		if(length+where>varray.Length)
			length = varray.Length - where;
		Vector3[] array = new Vector3[length];
		for(int x=0;x<length;x++)
		{
			if(x+where<varray.Length)
				array[x] = varray[where+x];
		}
		return array;
	}
	
	public string[] sarrayGet(int where, int length, bool ignoreErrors = false)
	{
		if(length<1 && !ignoreErrors)
		{
			length = sarray.Length;
			Debug.LogWarning("You just asked for an array of length < 1");
		}
		if(length+where>sarray.Length)
			length = sarray.Length - where;
		string[] array = new string[length];
		for(int x=0;x<length;x++)
		{
			if(x+where<sarray.Length)
				array[x] = sarray[where+x];
		}
		return array;
	}

	public float farrayGet(int where)
	{
		if(where<farray.Length && where>=0)
		{
			return farray[where];
		}
		else
			return fNull;
	}

	public int iarrayGet(int where)
	{
		if(where<iarray.Length && where>=0)
		{
			return iarray[where];
		}
		else
			return iNull;
	}

	public bool barrayGet(int where)
	{
		if(where<barray.Length && where>=0)
		{
			return barray[where];
		}
		else
			return bNull;
	}

	public Vector3 varrayGet(int where)
	{
		if(where<varray.Length && where>=0)
		{
			return varray[where];
		}
		else
			return vNull;
	}

	public String sarrayGet(int where)
	{
		if(where<sarray.Length && where>=0)
		{
			return sarray[where];
		}
		else
			return sNull;
	}

	public void farrayResize(int newlength)
	{
		float[] oldarray = farray;
		farray = new float[newlength];
		for(int x=0;x<newlength;x++)
			farray[x] = fNull;
		insertAt (0, oldarray);
	}

	public void iarrayResize(int newlength)
	{
		int[] oldarray = iarray;
		iarray = new int[newlength];
		for(int x=0;x<newlength;x++)
			iarray[x] = iNull;
		insertAt (0, oldarray);
	}

	public void barrayResize(int newlength)
	{
		bool[] oldarray = barray;
		barray = new bool[newlength];
		for(int x=0;x<newlength;x++)
			barray[x] = bNull;
		insertAt (0, oldarray);
	}

	public void varrayResize(int newlength)
	{
		Vector3[] oldarray = varray;
		varray = new Vector3[newlength];
		for(int x=0;x<newlength;x++)
			varray[x] = vNull;
		insertAt (0, oldarray);
	}

	public void sarrayResize(int newlength)
	{
		string[] oldarray = sarray;
		sarray = new string[newlength];
		for(int x=0;x<newlength;x++)
			sarray[x] = sNull;
		insertAt (0, oldarray);
	}

	public void allResize(int newlength)
	{
		farrayResize (newlength);
		iarrayResize (newlength);
		barrayResize (newlength);
		varrayResize (newlength);
		sarrayResize (newlength);
	}

	bool checkForBadStrings()
	{
		for(int x=0;x<sarray.Length;x++)
		{
			if(sarray[x].IndexOf(";") != -1 || sarray[x].IndexOf("|") != -1)
				return true;
		}
		return false;
	}

	public void resizeFIBVS(int f, int i, int b, int v, int s)
	{
		farrayResize (f);
		iarrayResize (i);
		barrayResize (b);
		varrayResize (v);
		sarrayResize (s);
	}

	void OnInspectorGUI() {

	}
}