using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerFSM : FiniteStateMachine
{
	public LazerFSM (StoppableObject parent) : base(parent)	{}

	protected override void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new LazerIdleState(this, waitTime));
	}

	protected override void AddDisableState(float waitTime)
	{
		states.Add(FSMState.Disable, new LazerDisableState(this, waitTime));
	}

}
