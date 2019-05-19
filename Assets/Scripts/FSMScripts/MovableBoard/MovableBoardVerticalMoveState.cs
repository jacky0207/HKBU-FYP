using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoardVerticalMoveState : MoveState
{
    private int direction = 1;

	private StoppableObject stoppableObject;

	public MovableBoardVerticalMoveState(FiniteStateMachine parent, float moveSpeed, Transform from, Transform to) : base(parent, moveSpeed, from, to)
	{
		stoppableObject = parent.GetParent();
	}
    
    public override void Update ()
    {
        // New position Y
        float deltaPositionY = moveSpeed * direction * Time.deltaTime;
        float boardNewPositionY = stoppableObject.transform.position.y + deltaPositionY;

        Vector3 boardPosition = stoppableObject.transform.position;

        // Change direction
        if (boardNewPositionY > to.position.y)
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.up * (to.position.y - boardPosition.y));
            }
            
            stoppableObject.transform.position = new Vector3(boardPosition.x, to.position.y, boardPosition.z);

            Transition(FSMState.Idle);
        }
        else if (boardNewPositionY < from.position.y)
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.up * (from.position.y - boardPosition.y));
            }

            stoppableObject.transform.position = new Vector3(boardPosition.x, from.position.y, boardPosition.z);

            Transition(FSMState.Idle);
        }
        // Movement
        else
        {
            // Move player
            // if (parent.collideWithPlayer)
            if (parent.GetParent().DetectPlayerAbove())
            {
                parent.player.Translate(Vector3.up * deltaPositionY);
            }

            // Change position
            stoppableObject.transform.position = new Vector3(boardPosition.x, boardNewPositionY, boardPosition.z);
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