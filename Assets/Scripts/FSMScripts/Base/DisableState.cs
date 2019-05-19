using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableState : State
{
	private float waitTime;
	private float counter;
	private StoppableObject stoppableObject;

	public DisableState(FiniteStateMachine parent, float waitTime) : base(parent)
	{
		this.waitTime = waitTime;
		stoppableObject = parent.GetParent();
	}

	protected override void StartUp ()
	{
		if (!beginning)
		{
			return;
		}

		// Action here
		beginning = false;
		stoppableObject.SetVisible(false);
	}
	
	public override void Update ()
	{
		StartUp();

		// idle stop
		if (counter < waitTime)
		{
			counter += Time.deltaTime;
		}
		else
		{
			Transition(FSMState.Idle);
		}
	}

	protected override void TransitionResetVariable()
	{
		counter = 0;
		beginning = true;
		stoppableObject.SetVisible(true);
	}

}