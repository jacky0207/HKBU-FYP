using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpikeSpawnPointFSM : FiniteStateMachine
{
	public FallSpikeSpawnPointFSM (StoppableObject parent) : base(parent) {}

	protected override void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new FallSpikeSpawnPointIdleState(this, waitTime));
	}

	protected override void AddTriggerState(float waitTime, int frequency = 1)
	{
		states.Add(FSMState.Trigger, new FallSpikeSpawnPointTriggerState(this, waitTime, frequency));
	}
}
