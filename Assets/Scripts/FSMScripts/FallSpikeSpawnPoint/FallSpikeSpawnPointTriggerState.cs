using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpikeSpawnPointTriggerState : TriggerState
{
	public FallSpikeSpawnPointTriggerState(FiniteStateMachine parent, float waitTime, int frequency) : base(parent, waitTime, frequency) {}
		
	protected override void Action1()
	{
		parent.GetParent().gameObject.GetComponent<FallSpikeSpawnPoint>().Spawn(times);
	}

}
