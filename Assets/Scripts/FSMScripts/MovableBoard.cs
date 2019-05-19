using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableBoard : StoppableObject
{
    public float idleWaitTime = 3.0f;
    public float moveSpeed = 5.0f;

    public Transform from;
    public Transform to;

    protected override void InitializeFSM()
    {
        fsm = new MovableBoardFSM(this);
    }

    protected override void AddFSMStates()
    {
        fsm.Idle(idleWaitTime).Move(moveSpeed, from, to);
    }

    void OnDrawGizmos()
    {
        if (from != null && to != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(from.position, to.position);
            // Handles.color = Color.yellow;
            // Handles.DrawAAPolyLine(20, previousPos, newPos);
        }
    }

}