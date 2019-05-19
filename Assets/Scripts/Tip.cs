using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tip : BaseMenu
{
	public Animator tipAnimator;
	private bool open;	// status of opening
	
	// Open panel when enter
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			StartCoroutine("OpenTip");
		}
	}

	private IEnumerator OpenTip()
	{
		// Set time scale 0
		Time.timeScale = 0;

		// Open panel
		tipAnimator.SetTrigger("Open");

		// Wait for opening
		while(!tipAnimator.GetCurrentAnimatorStateInfo(0).IsName("Open") || tipAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			yield return null;
		}

		// Status to open
		open = true;
	}

	// Disable panel
	public IEnumerator CloseTip()
	{
		// Status to open
		open = false;

		// Close panel
		tipAnimator.SetTrigger("Close");

		// Wait for closing
		while(!tipAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close") || tipAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			yield return null;
		}

		// Set time scale 0
		Time.timeScale = 1;

		// Destroy gameObject
		Destroy(gameObject);
	}

	// Detect touch
	void Update()
	{
		// Detect touch when open
		if (open)
		{
			if (Input.touchCount > 0)
			{
				StartCoroutine("CloseTip");
			}
		}
	}
}
