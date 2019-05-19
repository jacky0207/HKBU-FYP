using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionInitializer : MonoBehaviour
{
	private Transform player;
	
	public Transform playerInitialPosition;
	public bool setPlayerInitialPosition = true;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Set player initial position
	void Start ()
	{
		if (playerInitialPosition != null && setPlayerInitialPosition)
		{
			player.position = playerInitialPosition.position;
		}
	}

}
