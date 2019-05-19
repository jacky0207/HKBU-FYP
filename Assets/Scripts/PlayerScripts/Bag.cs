using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
	private ItemGUI itemGUI;
	private List<ItemColor> keys = new List<ItemColor>();
	private Dictionary<ItemColor, int> jewellarys = new Dictionary<ItemColor, int>();

	private void Awake()
	{
		itemGUI = GameObject.FindGameObjectWithTag("ItemGUI").GetComponent<ItemGUI>();
	}

	public List<ItemColor> GetAllKeys()
	{
		return keys;
	}

	public Dictionary<ItemColor, int> GetAllJewellarys()
	{
		return jewellarys;
	}

	public void AddItem(Key key)
	{	
		if (keys.Contains(key.ItemColor))
		{			
			Debug.LogWarning("Usage: key " + key.ItemColor + " exist");
		}
		else
		{
			keys.Add(key.ItemColor);
			UpdateItemGUI(key);

			Destroy(key.gameObject);	// Destroy object
		}
	}

	public void AddList(List<ItemColor> keys)
	{
		this.keys = keys;
		itemGUI.UpdateList(keys); // update ui
	}

	public void AddList(Dictionary<ItemColor, int> jewellarys)
	{
		this.jewellarys = jewellarys;
		itemGUI.UpdateList(jewellarys); // update ui
	}

	public void AddItem(Jewellary jewellary)
	{		
		if (jewellarys.ContainsKey(jewellary.ItemColor))
		{			
			Debug.LogWarning("Usage: jewellary " + jewellary.ItemColor + " exist");
		}
		else
		{
			jewellarys.Add(jewellary.ItemColor, jewellary.score);
			UpdateItemGUI(jewellary);

			Destroy(jewellary.gameObject);	// Destroy object
		}
	}

	private void UpdateItemGUI(Key key)
	{
		// Debug.Log("Get");
		itemGUI.UpdateItems(key);
	}

	private void UpdateItemGUI(Jewellary jewellary)
	{
		// Debug.Log("Get");
		itemGUI.UpdateItems(jewellary);
	}

}
