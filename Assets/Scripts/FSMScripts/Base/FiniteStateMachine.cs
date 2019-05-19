using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSMState
{
	Idle,
	Move,
	Disable,
	Trigger,
	Patrol	// a* path finding movement
}

public class FiniteStateMachine
{
	private StoppableObject parent;

	protected Dictionary<FSMState, State> states = new Dictionary<FSMState, State>();

	private State currentState;

    public Transform player { get; set; }
    // public bool collideWithPlayer { get; set; }

	public FiniteStateMachine (StoppableObject parent)
	{
		this.parent = parent;
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public FiniteStateMachine Idle(float waitTime)
	{
		if (states.ContainsKey(FSMState.Idle))
		{			
			Debug.LogWarning("Usage: state " + FSMState.Idle + " exist");
		}
		else
		{
			AddIdleState(waitTime);
			SetDefaultInitialState();
		}
		return this;
	}

	public FiniteStateMachine Move(float moveSpeed, Transform from, Transform to)
	{
		if (states.ContainsKey(FSMState.Move))
		{			
			Debug.LogWarning("Usage: state " + FSMState.Move + " exist");
		}
		else
		{
			AddMoveState(moveSpeed, from, to);
			SetDefaultInitialState();
		}
		return this;
	}

	public FiniteStateMachine Disable(float waitTime)
	{
		if (states.ContainsKey(FSMState.Disable))
		{			
			Debug.LogWarning("Usage: state " + FSMState.Disable + " exist");
		}
		else
		{
			AddDisableState(waitTime);
			SetDefaultInitialState();
		}
		return this;
	}

	public FiniteStateMachine Trigger(float waitTime, int frequency = 1)
	{
		if (states.ContainsKey(FSMState.Trigger))
		{			
			Debug.LogWarning("Usage: state " + FSMState.Trigger + " exist");
		}
		else
		{
			AddTriggerState(waitTime, frequency);
			SetDefaultInitialState();
		}
		return this;
	}

	public FiniteStateMachine Patrol(Dictionary<string, float> data)
	{
		if (states.ContainsKey(FSMState.Patrol))
		{			
			Debug.LogWarning("Usage: state " + FSMState.Patrol + " exist");
		}
		else
		{
			AddPatrolState(data);
			SetDefaultInitialState();
		}
		return this;
	}

	protected virtual void AddIdleState(float waitTime)
	{
		states.Add(FSMState.Idle, new IdleState(this, waitTime));
	}

	protected virtual void AddMoveState(float moveSpeed, Transform from, Transform to)
	{
		states.Add(FSMState.Move, new MoveState(this, moveSpeed, from, to));
	}

	protected virtual void AddDisableState(float waitTime)
	{
		states.Add(FSMState.Disable, new DisableState(this, waitTime));
	}

	protected virtual void AddTriggerState(float waitTime, int frequency = 1)
	{
		states.Add(FSMState.Trigger, new TriggerState(this, waitTime, frequency));
	}

	protected virtual void AddPatrolState(Dictionary<string, float> data)
	{
		states.Add(FSMState.Patrol, new PatrolState(this, data));
	}

	private void SetDefaultInitialState()
	{
		if (states.Count == 1)
		{
			currentState = states[0];
		}
	}

	public FiniteStateMachine InitialState(FSMState state)
	{
		if (states.ContainsKey(state))
		{
			currentState = states[state];
		}
		else
		{
			Debug.LogError("Usage: state " + state + " not exist");
		}
		return this;
	}
	
	public void Update ()
	{
		currentState.Update();
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        currentState.OnTriggerEnter2D(col);
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        currentState.OnTriggerStay2D(col);
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        currentState.OnTriggerExit2D(col);
    }
	
	public void OnDrawGizmos()
	{
		currentState.OnDrawGizmos();
	}

	public void SetCurrentState(FSMState state)
	{
		currentState = states[state];
	}

	public StoppableObject GetParent()
	{
		return parent;
	}

}