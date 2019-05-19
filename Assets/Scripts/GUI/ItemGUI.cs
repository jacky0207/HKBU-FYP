using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGUI : MonoBehaviour
{
    public GameObject redKey, greenKey, blueKey;
    
    public GameObject redJewellary, greenJewellary, blueJewellary;

    public void UpdateItems(Key key)
    {
		switch(key.ItemColor)
		{
			case ItemColor.red:
				redKey.SetActive(true);
                break;
			case ItemColor.green:
				greenKey.SetActive(true);
                break;
			case ItemColor.blue:
				blueKey.SetActive(true);
                break;
			default:
                break;
		}
    }

    public void UpdateItems(Jewellary jewellary)
    {
		switch(jewellary.ItemColor)
		{
			case ItemColor.red:
				redJewellary.SetActive(true);
                break;
			case ItemColor.green:
				greenJewellary.SetActive(true);
                break;
			case ItemColor.blue:
				blueJewellary.SetActive(true);
                break;
			default:
                break;
		}
    }

		public void UpdateList(List<ItemColor> keys)
		{
			// Add score if collect
			if (keys.Contains(ItemColor.red))
			{
				redKey.SetActive(true);
			}

			if (keys.Contains(ItemColor.green))
			{
				greenKey.SetActive(true);
			}

			if (keys.Contains(ItemColor.blue))
			{
				blueKey.SetActive(true);
			}
		}

		public void UpdateList(Dictionary<ItemColor, int> jewellerys)
		{
			// Add score if collect
			if (jewellerys.ContainsKey(ItemColor.red))
			{
				redJewellary.SetActive(true);
			}

			if (jewellerys.ContainsKey(ItemColor.green))
			{
				greenJewellary.SetActive(true);
			}

			if (jewellerys.ContainsKey(ItemColor.blue))
			{
				blueJewellary.SetActive(true);
			}
		}

}