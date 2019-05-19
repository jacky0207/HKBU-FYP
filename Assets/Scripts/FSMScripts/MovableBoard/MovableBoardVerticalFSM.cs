using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoardVerticalFSM : FiniteStateMachine
{
	public MovableBoardVerticalFSM (StoppableObject parent) : base(parent) {}

	protected override void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new MovableBoardIdleState(this, waitTime));
	}

	protected override void AddMoveState(float moveSpeed, Transform from, Transform to)
	{
		states.Add(FSMState.Move, new MovableBoardVerticalMoveState(this, moveSpeed, from, to));
	}
	
}