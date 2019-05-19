using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class MovableBoardVertical : StoppableObject
{
    public float idleWaitTime = 3.0f;
    public float moveSpeed = 5.0f;

    public Transform button;
    public Transform top;

    protected override void InitializeFSM()
    {
        fsm = new MovableBoardVerticalFSM(this);
    }

    protected override void AddFSMStates()
    {
        fsm.Idle(idleWaitTime).Move(moveSpeed, button, top);
    }

    void OnDrawGizmos()
    {
        if (button != null && top != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(button.position, top.position);
            // Handles.color = Color.yellow;
            // Handles.DrawAAPolyLine(20, button.position, top.position);
        }
    }

}