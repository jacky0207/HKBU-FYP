using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPointFSM : FiniteStateMachine
{
	public BulletSpawnPointFSM (StoppableObject parent) : base(parent) {}

	protected override void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new BulletSpawnPointIdleState(this, waitTime));
	}

	protected override void AddTriggerState(float waitTime, int frequency = 1)
	{
		states.Add(FSMState.Trigger, new BulletSpawnPointTriggerState(this, waitTime, frequency));
	}
}
