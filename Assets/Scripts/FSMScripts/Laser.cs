using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : StoppableObject
{
    public float idleWaitTime = 3.0f;
    public float disableWaitTime = 3.0f;

    protected override void InitializeFSM()
    {
        fsm = new LazerFSM(this);
    }

    protected override void AddFSMStates()
    {
        fsm.Idle(idleWaitTime).Disable(disableWaitTime);
    }

}