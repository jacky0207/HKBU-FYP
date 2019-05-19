using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterBorder : MonoBehaviour
{
	public Transform up;
	public Transform button;
	public Transform left;
	public Transform right;

	public float upBorder()
	{
		return up.position.y;
	}

	public float buttonBorder()
	{
		return button.position.y;
	}

	public float leftBorder()
	{
		return left.position.x;
	}

	public float rightBorder()
	{
		return right.position.x;
	}

	public float BorderHeight()
	{
		return up.position.y - button.position.y;
	}
	
}
