using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour
{
	public int row = 16;
	public int column = 16;
	public float nodeWidth;
	public bool showGrid = true;

	private Node[,] nodes;
	GameObject[] patrolPoints;

	protected void Awake ()
	{
		initializeNodes ();
	}

	private void initializeNodes ()
	{
		nodes = new Node[row, column];

		int index = 0;
		int gridRow = 0;
		int gridColumn = 0;

		// Node
		while (index < row * column)
		{
			gridRow = GetNodeRow (index);
			gridColumn = GetNodeColumn (index);
			Vector3 position = GetNodePosition (index);
			nodes [gridRow, gridColumn] = new Node (position);

			index++;
		}

		// Detect by raycast
		// Obstacle
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach (var obstacle in obstacles)
		{
			index = GetNodeIndex (obstacle.transform.position);
			gridRow = GetNodeRow (index);
			gridColumn = GetNodeColumn (index);
			nodes [gridRow, gridColumn].MarkAsObstacle ();
		}

		// Raycast detect method
		// index = 0;

		// while (index < row * column)
		// {
		// 	gridRow = GetNodeRow (index);
		// 	gridColumn = GetNodeColumn (index);
		// 	Vector3 nodePosition = nodes [gridRow, gridColumn].Position;

		// 	Vector3 startPoint = new Vector3 (nodePosition.x, 
		// 										nodePosition.y,
		// 										-10);

		// 	Vector3 direction = Vector3.forward;

		// 	float distance = 20;
		// 	int groundBitMask = 8;
		// 	int damagableBitMask = 10;
		// 	int layerMask = (1 << groundBitMask) | (1 << damagableBitMask);

		// 	// out hit
		// 	RaycastHit2D hit = Physics2D.Raycast(startPoint,
		// 										direction, 
		// 										distance, 
		// 										layerMask);

		// 	// Debug raycast
		// 	// Debug.DrawRay(startPoint, direction * 20, Color.yellow);

		// 	// Detect damagable object and ground
		// 	if (hit.collider != null)
		// 	{
		// 		// marking as obstacle
		// 		// Debug.Log(hit.transform.gameObject);
		// 		nodes[gridRow, gridColumn].MarkAsObstacle();
		// 	}

		// 	// increment
		// 	index++;
		// }

		// Patrol Point
		patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
		foreach (var patrolPoint in patrolPoints)
		{
			index = GetNodeIndex (patrolPoint.transform.position);
			gridRow = GetNodeRow (index);
			gridColumn = GetNodeColumn (index);
			nodes [gridRow, gridColumn].MarkAsPatrolPoint ();
		}
	}

	private Vector3 GetNodePosition (int index)	// Center world position
	{
		int gridRow = GetNodeRow (index);
		int gridColumn = GetNodeColumn (index);
		float localPositionX = nodeWidth / 2.0f + nodeWidth * gridColumn;
		// float localPositionZ = -1 * nodeWidth / 2.0f + -1 * nodeWidth * gridRow;
		float localPositionY = -1 * nodeWidth / 2.0f + -1 * nodeWidth * gridRow;
		// Vector3 localPosition = new Vector3 (localPositionX, 0, localPositionZ);
		Vector3 localPosition = new Vector3 (localPositionX, localPositionY, 0);
		return transform.position + localPosition;
	}

	private int GetNodeIndex (Vector3 position)
	{
		Vector3 localPosition = position - transform.position;
		// int gridRow = (int) (localPosition.z / (-1 * nodeWidth));
		int gridRow = (int) (localPosition.y / (-1 * nodeWidth));
		int gridColumn = (int) (localPosition.x / nodeWidth);
		int index = gridRow * column + gridColumn;
		// Debug.Log(index);
		return index;
	}

	private int GetNodeRow (int index)
	{
		int gridRow = index / column;
		return gridRow;
	}

	private int GetNodeColumn (int index)
	{
		int gridColumn = index % column;
		return gridColumn;
	}

	private void AssignNeighbourNode (int gridRow, int gridColumn, ArrayList neighbourNodes)
	{
		// Out of range
		if (gridRow < 0 || gridRow >= row || gridColumn < 0 || gridColumn >= column)
		{
			return;
		}

		// if not obstacle
		if (!nodes [gridRow, gridColumn].Obstacle)
		{
			neighbourNodes.Add (nodes [gridRow, gridColumn]);
		}
	}

	public ArrayList GetNeighbourNodes (Node node)
	{
		ArrayList neighbourNodes = new ArrayList ();

		int index = GetNodeIndex (node.Position);
		int gridRow = GetNodeRow (index);
		int gridColumn = GetNodeColumn (index);

		// 8 node arround that node
		// Top
		AssignNeighbourNode (gridRow - 1, gridColumn, neighbourNodes);
		// Top Right
		AssignNeighbourNode (gridRow - 1, gridColumn + 1, neighbourNodes);
		// Right
		AssignNeighbourNode (gridRow, gridColumn + 1, neighbourNodes);
		// Bottom Right
		AssignNeighbourNode (gridRow + 1, gridColumn + 1, neighbourNodes);
		// Bottom
		AssignNeighbourNode (gridRow + 1, gridColumn, neighbourNodes);
		// Bottom Left
		AssignNeighbourNode (gridRow + 1, gridColumn - 1, neighbourNodes);
		// Left
		AssignNeighbourNode (gridRow, gridColumn - 1, neighbourNodes);
		// Top Left
		AssignNeighbourNode (gridRow - 1, gridColumn - 1, neighbourNodes);

		return neighbourNodes;
	}
	
	public Node GetNode (Vector3 position)
	{
		int index = GetNodeIndex (position);
		int gridRow = GetNodeRow (index);
		int gridColumn = GetNodeColumn (index);
		return nodes [gridRow, gridColumn];
	}

	public Node GetRandomPatrolPoint ()
	{
		Vector3 patrolPointPosition = patrolPoints [Random.Range (0, patrolPoints.Length)].transform.position;
		return GetNode (patrolPointPosition);
	}

	public void ClearNodeHistory ()
	{
		// For next time checking shortest path
		for (int gridRow = 0; gridRow < row; gridRow++)
		{
			for (int gridColumn = 0; gridColumn < column; gridColumn++)
			{
				nodes [gridRow, gridColumn].parentNode = null;
				nodes [gridRow, gridColumn].gCost = 0.0f;
				nodes [gridRow, gridColumn].fCost = 0.0f;
			}
		}
	}

	// Debug grid layout
	protected void OnDrawGizmos ()
	{
		if (showGrid)
		{
			Vector3 position = transform.position;
			// Row
			for (int gridRow = 0; gridRow <= row; gridRow++)
			{
				// Debug.DrawLine (position + new Vector3 (0, 0, -1 * nodeWidth * gridRow), position + new Vector3 (nodeWidth * column, 0, -1 * nodeWidth * gridRow), Color.black);
				// centre position + current row position
				Debug.DrawLine (position + new Vector3 (0, -1 * nodeWidth * gridRow, 0), 
								position + new Vector3 (nodeWidth * column, -1 * nodeWidth * gridRow, 0), 
								Color.black);
			}
			// Column
			for (int gridColumn = 0; gridColumn <= column; gridColumn++)
			{
				// Debug.DrawLine (position + new Vector3 (nodeWidth * gridColumn, 0, 0), position + new Vector3 (nodeWidth * gridColumn, 0, -1 * nodeWidth * row), Color.black)
				Debug.DrawLine (position + new Vector3 (nodeWidth * gridColumn, 0, 0), 
								position + new Vector3 (nodeWidth * gridColumn, -1 * nodeWidth * row, 0), 
								Color.black);;
			}
		}
	}

}