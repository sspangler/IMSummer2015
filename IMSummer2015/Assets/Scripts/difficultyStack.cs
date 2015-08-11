using UnityEngine;
using System.Collections;

public class difficultyStack : MonoBehaviour {

	public int[] stack = new int[]{1,1,2,3,2};
	int[,] stacks = new int[,]{{1,2,1,2,3},
		{1,1,2,3,2}, {1,2,2,1,3}, {1,2,3,2,1}};

	// Use this for initialization
	void Awake () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public int pop()
	{
		if(stack.Length<1)
		{
			stackMore();
		}

		int toReturn = stack [0];
		removeNext ();

		return toReturn;
	}

	public void stackMore()
	{
		int[] addStack = new int[5];
		int currentStack = Random.Range (0, 4);
		for(int x=0;x<addStack.Length;x++)
			addStack[x] = stacks[currentStack,x];

		append (addStack);
	}

	public void removeNext()
	{
		int[] newArray = new int[stack.Length - 1];
		for (int x=1; x<stack.Length; x++)
			newArray [x - 1] = stack [x];
		stack = newArray;
	}

	public void append(int[] newStack)
	{
		int[] newArray = new int[stack.Length + newStack.Length];
		for(int x=0;x<stack.Length;x++)
			newArray[x] = stack[x];
		for(int x=stack.Length;x<newArray.Length;x++)
			newArray[x] = newStack[x-stack.Length];
		stack = newArray;
	}

	public void reset()
	{
		stack = new int[]{1,1,2,3,2};
	}
}
