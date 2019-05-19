using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerState : State
{
	protected float waitTime;
	protected float frequency;

	protected int times;
	protected float counter = 9999;
	

	public TriggerState(FiniteStateMachine parent, float waitTime, int frequency) : base(parent)
	{
		this.waitTime = waitTime;
		this.frequency = frequency;
	}
	
	public override void Update ()
	{
		// Do action
		if (times < frequency)
		{
			// Action waiting
			if (counter < waitTime)
			{
				counter += Time.deltaTime;
			}
			// Next action
			else
			{
				// Code here
				Action1();
				
				// Reset variable
				ActionSetVariable();
			}
		}
		// Finish
		else
		{
			Transition1();
		}
	}

	protected virtual void Action1()
	{
		
	}

	protected virtual void Transition1()
	{
		Transition(FSMState.Idle);
	}

	protected virtual void ActionSetVariable()
	{
		counter = 0;
		times += 1;
	}

	protected override void TransitionResetVariable()
	{
		counter = 9999;
		times = 0;
	}
}
