using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVIdleState : IdleState
{
	public CCTVIdleState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime) {}

	protected override void Transition1()
	{
		Transition(FSMState.Patrol);
	}
}
