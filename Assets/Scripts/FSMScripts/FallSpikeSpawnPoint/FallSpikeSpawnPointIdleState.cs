using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpikeSpawnPointIdleState : IdleState
{
	public FallSpikeSpawnPointIdleState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime) {}

	protected override void Transition1()
	{
		Transition(FSMState.Trigger);
	}
}
