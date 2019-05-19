using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoardMoveState : MoveState
{
    private int direction = 1;

	private StoppableObject stoppableObject;

	public MovableBoardMoveState(FiniteStateMachine parent, float moveSpeed, Transform from, Transform to) : base(parent, moveSpeed, from, to)
	{
		stoppableObject = parent.GetParent();
	}
    
    public override void Update ()
    {
        // New position X
        float deltaPositionX = moveSpeed * direction * Time.deltaTime;
        float boardNewPositionX = stoppableObject.transform.position.x + deltaPositionX;

        Vector3 boardPosition = stoppableObject.transform.position;

        // Change direction
        if (boardNewPositionX > to.position.x)
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.right * (to.position.x - boardPosition.x));
            }

            // Change position
            stoppableObject.transform.position = new Vector3(to.position.x, boardPosition.y, boardPosition.z);

            Transition(FSMState.Idle);
        }
        else if (boardNewPositionX < from.position.x)
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.right * (from.position.x - boardPosition.x));
            }

            // Change position
            stoppableObject.transform.position = new Vector3(from.position.x, boardPosition.y, boardPosition.z);

            Transition(FSMState.Idle);
        }
        // Movement
        else
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.right * deltaPositionX);
            }

            // Change position
            stoppableObject.transform.position = new Vector3(boardNewPositionX, boardPosition.y, boardPosition.z);
        }
    }

    protected override void TransitionResetVariable()
    {
        direction *= -1;
    }

    // public override void OnTriggerEnter2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         parent.collideWithPlayer = true;
    //     }
    // }

    // public override void OnTriggerExit2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         parent.collideWithPlayer = false;
    //     }
    // }
}