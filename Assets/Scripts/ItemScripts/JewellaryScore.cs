using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewellaryScore : MonoBehaviour
{
	public int redScore = 500;
	public int greenScore = 1000;
	public int blueScore = 1500;
	
    void Start()
    {
        Object[] objects = Object.FindObjectsOfType(typeof(GameObject));
        foreach (GameObject item in objects)
        {
            Jewellary jewellary = item.GetComponent<Jewellary>();
            if (jewellary != null)
            {
				jewellary.score = GetScore(jewellary.ItemColor); 
            }
        }
    }

	public int GetScore(ItemColor ItemColor)
	{
		switch(ItemColor)
		{
			case ItemColor.red:
				return redScore;
			case ItemColor.green:
				return greenScore;
			case ItemColor.blue:
				return blueScore;
			default:
				return 0;
		}
	}

}
