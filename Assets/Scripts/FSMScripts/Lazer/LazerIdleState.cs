using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerIdleState : IdleState
{
	private AudioSource audioSource;

	public LazerIdleState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime) 
	{
		audioSource = parent.GetParent().GetComponent<AudioSource>();
	}

	protected override void TransitionResetVariable()
	{
		base.TransitionResetVariable();

		// sound
		audioSource.Stop();
	}

}
