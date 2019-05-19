using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
	public float timeLimit;
	private float remainingTime;
    public Text timeText;
	public GameObject terminator;
	private float terminatorHeight;
    // private float timeSinceStart;

	private bool on = true;
	void OnEnable()
	{
		timeText.text = timeLimit.ToString("0.00");
		remainingTime = timeLimit;
	}

	void Start()
	{
		terminatorHeight = terminator.GetComponent<SpriteRenderer>().bounds.size.y;
	}

	void Update()
	{
		// Count time in normal time speed
		if (on)
		{
			CountTime();
			terminator.transform.position += Vector3.down * terminatorHeight * 0.8f * Time.deltaTime / timeLimit;	// 0.8 for not cover whole of screen	
		}
	}

	private void CountTime()
	{
		// timeSinceStart += Time.deltaTime;
		remainingTime -= Time.deltaTime;

		// Update gui
		// int minute = (int)(timeSinceStart / 60);
		// int second = (int)(timeSinceStart % 60);

		timeText.text = remainingTime.ToString("0.00");
	}

	// To oo/oof time count
	public void SetOn(bool value)
	{
		on = value;
	}

}
