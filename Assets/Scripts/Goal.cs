using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	public bool redKey = true;
	public bool greenKey = true;
	public bool blueKey = true;
    public GameObject doorSignal;

	// Red/Green door
	public Transform translatePosition;
	public Transform cameraSource;
	public Transform cameraDestination;

	// Trigger variable
	private bool disable;	// True after used once

	public bool CanOpen(Bag bag)
	{
		if (disable)	// once opened
		{
			return false;
		}
		else	// depends on key owned
		{
			bool result = ExistAllKeys(bag);
			doorSignal.SetActive(result);
			return result;
		}
	}

	private bool ExistAllKeys(Bag bag)
	{
		List<ItemColor> keys = bag.GetAllKeys();
		
		return (!redKey || keys.Contains(ItemColor.red)) && 
			(!greenKey || keys.Contains(ItemColor.green)) && 
			(!blueKey || keys.Contains(ItemColor.blue));
	}

	public void LeaveDoor()
	{
		doorSignal.SetActive(false);
	}

	public void DisableDoor(bool sound = true)	// Open door action
								// Not allow open again
	{
		disable = true;
	
		// Sound		
		if (sound) GetComponent<SoundManager>().PlayOnce("door");
	}

}
