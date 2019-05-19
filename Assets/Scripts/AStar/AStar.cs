using UnityEngine;
using System.Collections;

public class AStar
{	
	private static ArrayList GetPathList (Node node)
	{
		ArrayList pathList = new ArrayList ();

		while (node != null)
		{
			pathList.Add (node);
			node = node.parentNode;
		}

		pathList.Reverse ();	// normal arrange i.e start->...->end
		return pathList;
	}

	private static float OneStepCost (Node currentNode, Node neighbourNode)	// Cost between two continuous nodes cost
	{
		float displacement = Vector3.Distance (currentNode.Position, neighbourNode.Position);
		return displacement * neighbourNode.CostWeight;
	}

	private static float HeuristicCost (Node currentNode, Node endNode)	// remaining distance cost
	{
		float displacement = Vector3.Distance (currentNode.Position, endNode.Position);
		return displacement;
	}

	// Limit two node should both in same area
	// Areas are continuously connected from left to right
	// Area start from (0,0,0) center
	public static ArrayList FindPath (Vector3 position, float areaWidth)
	{
		// GridManager gridManager = GameObject.FindGameObjectWithTag ("GridManager").GetComponent <GridManager> ();
		GridManager gridManager = GameObject.FindObjectOfType(typeof(GridManager)) as GridManager;
		Node startNode = gridManager.GetNode (position);
		Node endNode = gridManager.GetRandomPatrolPoint ();

		bool samePosition = startNode == endNode;
		bool tooNear = Vector3.Distance (startNode.Position, endNode.Position) < gridManager.nodeWidth;

		float startPositionX = startNode.Position.x - areaWidth / 2;	// 1st area result in -ve value
		float endPositionX = endNode.Position.x - areaWidth / 2;	// 1st area result in -ve value
		int startArea = (startPositionX < 0) ? -1 : (int)(startPositionX / areaWidth);	// output = 0,1,2...
		int endArea = (endPositionX < 0) ? -1 : (int)(endPositionX / areaWidth);	// output = 0,1,2...

		bool sameArea = startArea == endArea;

		// Not same node, not too near and same area
		while (samePosition || tooNear || !sameArea)
		{
			// Get node again
			endNode = gridManager.GetRandomPatrolPoint ();

			// Calculate again
			samePosition = startNode == endNode;
			tooNear = Vector3.Distance (startNode.Position, endNode.Position) < gridManager.nodeWidth;

			endPositionX = endNode.Position.x - areaWidth / 2;	// 1st area result in -ve value
			endArea = (endPositionX < 0) ? -1 : (int)(endPositionX / areaWidth);	// output = 0,1,2...
			
			sameArea = startArea == endArea;
		}

		return AStar.FindPath (startNode, endNode);
	}

	public static ArrayList FindPath (Vector3 position)
	{
		// GridManager gridManager = GameObject.FindGameObjectWithTag ("GridManager").GetComponent <GridManager> ();
		GridManager gridManager = GameObject.FindObjectOfType(typeof(GridManager)) as GridManager;
		Node startNode = gridManager.GetNode (position);
		Node endNode = gridManager.GetRandomPatrolPoint ();

		while (startNode == endNode || Vector3.Distance (startNode.Position, endNode.Position) < gridManager.nodeWidth)	// alreadly in the destination or too near
		{
			endNode = gridManager.GetRandomPatrolPoint ();
		}

		return AStar.FindPath (startNode, endNode);
	}

	public static ArrayList FindPath (Vector3 position, Vector3 destination)
	{
		// GridManager gridManager = GameObject.FindGameObjectWithTag ("GridManager").GetComponent <GridManager> ();
		GridManager gridManager = GameObject.FindObjectOfType(typeof(GridManager)) as GridManager;
		Node startNode = gridManager.GetNode (position);
		Node endNode = gridManager.GetNode (destination);
		if (position == destination)	// alreadly in the destination
		{
			Debug.LogWarning("Alreadly arrive destination");
			return null;
		}
		return AStar.FindPath (startNode, endNode);
	}
	
	public static ArrayList FindPath (Node startNode, Node endNode)
	{
		NodeList openList = new NodeList ();	// New neighbour node
		NodeList closeList = new NodeList ();	// Node alreadly processed

		// GridManager gridManager = GameObject.FindGameObjectWithTag ("GridManager").GetComponent <GridManager> ();
		GridManager gridManager = GameObject.FindObjectOfType(typeof(GridManager)) as GridManager;
		gridManager.ClearNodeHistory ();

		// Add the start node
		startNode.gCost = 0;
		startNode.fCost = startNode.gCost + HeuristicCost (startNode, endNode);
		openList.Push (startNode);

		while (openList.GetFirstNode () != null)
		{
			Node currentNode = openList.GetFirstNode ();

			if (currentNode == endNode)
			{
				return GetPathList (endNode);
			}

			ArrayList neighbourNodes = gridManager.GetNeighbourNodes (currentNode);

			foreach (Node neighbourNode in neighbourNodes)
			{
				if (!closeList.Contain (neighbourNode))	// Skip if processed before
				{
					float gCost = currentNode.gCost + OneStepCost (currentNode, neighbourNode);
					float fCost = gCost + HeuristicCost (neighbourNode, endNode);

					// Update if neighbour node is not contained in openlist
					// or new fCost is lower than the before
					if (!openList.Contain (neighbourNode) || fCost < neighbourNode.fCost)
					{
						neighbourNode.parentNode = currentNode;
						neighbourNode.gCost = gCost;
						neighbourNode.fCost = fCost;

						// Add if neighbour node is not contained in openlist
						if (!openList.Contain (neighbourNode))
						{
							openList.Push (neighbourNode);
						}
					}
				}
			}

			// Manipulate list
			openList.Pop (currentNode);
			closeList.Push (currentNode);
		}

		return GetPathList (endNode);
	}

}