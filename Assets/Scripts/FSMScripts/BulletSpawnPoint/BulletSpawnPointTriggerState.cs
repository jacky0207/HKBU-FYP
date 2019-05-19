using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPointTriggerState : TriggerState
{
	private SoundManager soundManager;

	public BulletSpawnPointTriggerState(FiniteStateMachine parent, float waitTime, int frequency) : base(parent, waitTime, frequency)
	{
		soundManager = parent.GetParent().GetComponent<SoundManager>();
	}
		
	protected override void Action1()
	{
		parent.GetParent().gameObject.GetComponent<BulletSpawnPoint>().Spawn(times);

		// sound
		soundManager.PlayOnce("cannon");
	}
}
