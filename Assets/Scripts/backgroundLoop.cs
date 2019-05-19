using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundLoop : MonoBehaviour
{
	public float moveSpeed = 3.0f;
	// public float bac
	public Transform background1;
	public Transform background2;
		
	void Update ()
	{
		// Move background
		background1.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		background2.transform.position += Vector3.left * moveSpeed * Time.deltaTime;

		// Allocate position
		if (background1.transform.position.x < -19.2f)
		{
			background1.transform.position = Vector3.right * 19.2f;
		} 
		else if (background2.transform.position.x < -19.2f)
		{
			background2.transform.position = Vector3.right * 19.2f;
		}
	}

}