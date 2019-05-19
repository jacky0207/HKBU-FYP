using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGUI : MonoBehaviour
{
    public Text timeText;
	public int idealTime;
    private float timeSinceStart;

	private bool on = true;

	void Update()
	{
		// Count time in normal time speed
		if (on)
		{
			CountTime();			
		}
	}

	private void CountTime()
	{
		timeSinceStart += Time.deltaTime;

		// Update gui
		int minute = (int)(timeSinceStart / 60);
		int second = (int)(timeSinceStart % 60);

		string minuteAppend = (minute < 10) ? "0" : "";
		string secondAppend = (second < 10) ? "0" : "";

		timeText.text = minuteAppend + minute + ":" + secondAppend + second;
	}

	// To oo/oof time count
	public void SetOn(bool value)
	{
		on = value;
	}

	// Get time
	public float GetTime()
	{
		return timeSinceStart;
	}

	// Get ideal time
	public int GetIdealTime()
	{
		return idealTime;
	}

	// Set time
	// Load game
	public void SetTime(float time)
	{
		timeSinceStart = time;

		// Update gui
		int minute = (int)(timeSinceStart / 60);
		int second = (int)(timeSinceStart % 60);

		string minuteAppend = (minute < 10) ? "0" : "";
		string secondAppend = (second < 10) ? "0" : "";
		
		timeText.text = minuteAppend + minute + ":" + secondAppend + second;
	}

}
