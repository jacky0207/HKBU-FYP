using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVFSM : FiniteStateMachine
{
	public CCTVFSM (StoppableObject parent) : base(parent) {}

	protected override void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new CCTVIdleState(this, waitTime));
	}

	protected override void AddPatrolState(Dictionary<string, float> data)
	{
		states.Add(FSMState.Patrol, new CCTVPatrolState(this, data));
	}

	protected override void AddTriggerState(float waitTime, int frequency = 1)
	{
		states.Add(FSMState.Trigger, new CCTVTriggerState(this, waitTime, frequency));
	}
}
