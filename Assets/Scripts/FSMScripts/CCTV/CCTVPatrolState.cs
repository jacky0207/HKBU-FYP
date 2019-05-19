using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVPatrolState : PatrolState
{
	public CCTVPatrolState(FiniteStateMachine parent, Dictionary<string, float> data) : base(parent, data) {}

	protected override void Transition1()
	{
		Transition(FSMState.Idle);
	}

	protected override void Transition2()
	{
		Transition(FSMState.Trigger);
	}

}
