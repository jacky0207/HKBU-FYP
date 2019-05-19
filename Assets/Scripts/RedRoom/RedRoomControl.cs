using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRoomControl : MonoBehaviour
{
    private CameraGrey cameraGrey;

	public GameObject countDown;
	public GameObject light;
	public GameObject stoppableObjects;

	void Awake()
	{		
        cameraGrey = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraGrey>(); // shader
	}

	// Activate object when player enter
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			countDown.SetActive(true);
			light.SetActive(true);
			stoppableObjects.SetActive(true);

			int childCount = stoppableObjects.transform.childCount;
			bool timeStop = cameraGrey.GetOn();

			if (timeStop)
			{
				for (int childIndex = 0; childIndex < childCount; childIndex++)
				{
					stoppableObjects.transform.GetChild(childIndex).GetComponent<StoppableObject>().SwitchOnOrOff(false);
				}
			}

			// Sound
			GetComponent<AudioSource>().Play();
		}
	}

	// Disactivate object when player exit
	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			countDown.SetActive(false);
			light.SetActive(false);
			stoppableObjects.SetActive(false);

			// Sound
			GetComponent<AudioSource>().Stop();
		}
	}

}