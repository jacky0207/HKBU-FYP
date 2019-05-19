using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected FiniteStateMachine parent;
	protected bool beginning = true;

	public State(FiniteStateMachine parent) { this.parent = parent; }
	protected virtual void StartUp() {}
	public virtual void Update() {}
    public virtual void OnTriggerEnter2D(Collider2D col) {}
    public virtual void OnTriggerStay2D(Collider2D col) {}
    public virtual void OnTriggerExit2D(Collider2D col) {}
    public virtual void OnDrawGizmos() {}
	protected virtual void Transition(FSMState state) { TransitionResetVariable(); parent.SetCurrentState(state); }
	protected virtual void TransitionResetVariable() {}

}