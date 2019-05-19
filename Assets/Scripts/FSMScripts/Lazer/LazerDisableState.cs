using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerDisableState : DisableState
{
	private AudioSource audioSource;

	public LazerDisableState(FiniteStateMachine parent, float waitTime) : base(parent, waitTime)
	{
		audioSource = parent.GetParent().GetComponent<AudioSource>();
	}

	protected override void TransitionResetVariable()
	{
		base.TransitionResetVariable();

		// sound
		audioSource.Play();
	}
}
