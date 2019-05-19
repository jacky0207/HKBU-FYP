using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable
{
	private Vector3 position;

	public Node parentNode;
	public float gCost;
	public float fCost;

	private bool obstacle;
	private bool patrolPoint;

	private float costWeight;

	public const float defaultCostWeight = 1.0f;

	public Vector3 Position { get { return position; } }

	public bool Obstacle { get { return obstacle; } }
	public bool PatrolPoint { get { return patrolPoint; } }

	public float CostWeight { get { return costWeight; } }

	public Node (Vector3 position)
	{
		this.position = position;
		parentNode = null;
		gCost = 0.0f;
		fCost = 0.0f;
		obstacle = false;
		patrolPoint = false;
		costWeight = defaultCostWeight;
	}

	public void MarkAsObstacle ()
	{
		obstacle = true;
	}

	public void MarkAsPatrolPoint ()
	{
		patrolPoint = true;
	}

	// Shortest cost priority
	public int CompareTo(object obj)
	{
		Node node = (Node)obj;
		if (this.fCost < node.fCost) // if this F < obj F, return -1
			return -1;
		if (this.fCost > node.fCost) // if this F > obj F, return 1
			return 1;

		return 0;                                    // if this F == obj F, return 0
	}

}