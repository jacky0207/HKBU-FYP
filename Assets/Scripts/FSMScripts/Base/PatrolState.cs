using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
	// Data
	protected float moveSpeed;
	protected float mass;
	protected float curSpeed;	// angular speed
	protected float reachRadius;	// Distance from node
	// protected float avoidDistance = 2.0f;	// Distance from obstacle
	// protected float avoidForce = 20.0f;
	protected float areaWidth;

	// A star
	private ArrayList path;
	private int pathIndex;
	
	// Calculated velocity
	private Vector3 velocity;

	// GameObject link
	private StoppableObject patrolObjectBase;
	private Transform patrolObjectTransform;

	public PatrolState(FiniteStateMachine parent, Dictionary<string, float> data) : base(parent)
	{
		this.moveSpeed = data["moveSpeed"];
		this.mass = data["mass"];
		this.curSpeed = data["curSpeed"];
		this.reachRadius = data["reachRadius"];
		this.areaWidth = data["areaWidth"];
		// this.avoidDistance = data["avoidDistance"];
		// this.avoidForce = data["avoidForce"];

		patrolObjectBase = parent.GetParent();
		patrolObjectTransform = patrolObjectBase.transform;

		// Get patrol path
		path = AStar.FindPath(parent.GetParent().transform.position, areaWidth);
		// Debug.Log(path);
	}
	
	public override void Update ()
	{
		// Check if reach next node
		if (ReachNextNode () || ReachDestination ())
		{
			pathIndex++;

			// check if reach destination
			if (pathIndex == path.Count - 1)
			{
				Transition1();
				return;
			}
		}
		Vector3 destination = ((Node)path [path.Count - 1]).Position;
		velocity += Steer (destination);

		patrolObjectTransform.position += velocity * moveSpeed * Time.deltaTime; //Move the vehicle according to the velocity
		// Quaternion lookRotation = Quaternion.LookRotation(velocity); //Rotate the vehicle towards the desired Velocity
		// patrolObjectTransform.rotation = Quaternion.Slerp (patrolObjectTransform.rotation, lookRotation, curSpeed * Time.deltaTime);
	}

	public bool ReachNextNode ()
	{
		Vector3 nextNodePos = ((Node)path [pathIndex + 1]).Position;
		float nextNodeDistance = Vector3.Distance (patrolObjectTransform.position, nextNodePos);
		return nextNodeDistance < reachRadius;
	}

	public bool ReachDestination ()
	{
		Vector3 destination = ((Node)path [path.Count - 1]).Position;
		float destinationDistance = Vector3.Distance (patrolObjectTransform.position, destination);
		return destinationDistance < reachRadius;
	}

	// Obstacle exist in straight line to destination
	// public bool ObstacleExist ()
	// {
	// 	RaycastHit hit;
	// 	int groundLayer = 8, damagableLayer = 10;
	// 	LayerMask layerMask = (1 << groundLayer) | (1 << damagableLayer);

	// 	Vector3 destination = ((Node)path [path.Count - 1]).Position;
	// 	Vector3 dir = destination - patrolObjectTransform.position;
	// 	dir.Normalize ();

	// 	// GridManager gridManager = GameObject.FindGameObjectWithTag ("GridManager").GetComponent<GridManager> ();
	// 	GridManager gridManager = GameObject.FindObjectOfType(typeof(GridManager)) as GridManager;
	// 	float sceneMaxDisplacement = Mathf.Sqrt (Mathf.Pow (gridManager.row, 2) * Mathf.Pow (gridManager.column, 2)) * gridManager.nodeWidth;

	// 	if (Physics.Raycast (patrolObjectTransform.position, dir, out hit, sceneMaxDisplacement, layerMask))
	// 	{
	// 		return true;
	// 	}
	// 	return false;
	// }

	//Steering algorithm to steer the vector towards the target
	public Vector3 Steer(Vector3 destination)
	{
		Vector3 dir = destination - patrolObjectTransform.position;

		// Go straight if straight line
		// if (ObstacleExist ())
		{
			// AStar next node
			destination = ((Node)path [pathIndex + 1]).Position;
			dir = destination - patrolObjectTransform.position;
		}

		Vector3 seek = dir.normalized;

		// Avoid if see enemy or wall
		// AvoidObstacles (ref seek);

		Vector3 steer = ( seek - velocity ) / mass;
		return steer;        
	}

	//Calculate the new directional vector to avoid the obstacle not marked
	// public void AvoidObstacles(ref Vector3 dir)
	// {
	// 	int patrolBitMask = 10;
	// 	int layerMask = 1 << patrolBitMask;

	// 	// out hit
	// 	RaycastHit2D hit = Physics2D.Raycast(patrolObjectTransform.position, 
	// 										dir, 
	// 										avoidDistance, 
	// 										layerMask);

	// 	// Detect damagable object and ground -> add avoid force
	// 	if (hit.collider != null)
	// 	{
	// 		Debug.Log("Hit");
	// 		Vector3 hitNormal = hit.normal;
	// 		// hitNormal.y = 0;
	// 		hitNormal.z = 0;
	// 		dir += hitNormal * avoidForce;
	// 	}
	// }

	protected virtual void Transition1()
	{
		Transition(FSMState.Idle);
	}

	protected override void TransitionResetVariable()
	{
		// Get patrol path again
		path = AStar.FindPath(patrolObjectTransform.position, areaWidth);
		pathIndex = 0;
	}

    // public override void OnTriggerEnter2D(Collider2D col)
    public override void OnTriggerStay2D(Collider2D col)
    {
		if (patrolObjectBase.GetOn())
        {
			// Collide player
			if (col.gameObject.tag == "Player")
			{
				// Change state
				Transition2();
			}
		}
    }

    // public override void OnTriggerExit2D(Collider2D col)
    // {
	// 	if (patrolObjectBase.GetOn())
    //     {
	// 		// Collide player
	// 		if (col.gameObject.tag == "Player")
	// 		{
	// 			// parent.collideWithPlayer = false;
	// 		}
	// 	}
    // }

	protected virtual void Transition2()
	{
		Transition(FSMState.Idle);
	}

	public override void OnDrawGizmos()
	{
		if (path == null)
		{
			return;
		}

		for (int index = 0; index < path.Count; index++)
		{
			if (index == path.Count - 1)
			{
				continue;
			}
			Debug.DrawLine (((Node) path [index]).Position, ((Node) path [index + 1]).Position, Color.red);
		}
	}

}
