using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPointIdleState : IdleState
{
	public BulletSpawnPointIdleState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime) {}

	protected override void Transition1()
	{
		Transition(FSMState.Trigger);
	}
}
