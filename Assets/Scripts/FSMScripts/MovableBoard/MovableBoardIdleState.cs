using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoardIdleState : IdleState
{
	public MovableBoardIdleState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime) {}

	protected override void Transition1()
	{
		Transition(FSMState.Move);
	}
	
}