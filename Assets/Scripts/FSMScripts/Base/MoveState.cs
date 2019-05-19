using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
	protected float moveSpeed;
    protected Transform from;
    protected Transform to;

	public MoveState(FiniteStateMachine parent, float moveSpeed, Transform from, Transform to) : base(parent)
	{
		this.moveSpeed = moveSpeed;
		this.from = from;
		this.to = to;
	}
}