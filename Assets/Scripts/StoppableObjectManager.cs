using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppableObjectManager : MonoBehaviour
{
    delegate void MultiDelegate(bool value);
    MultiDelegate myMultiDelegate;

    delegate void VisibleChangeClassDelegate();
    VisibleChangeClassDelegate visibleChange;

    public void AddObject(StoppableObject stoppableObject)
    {
        myMultiDelegate += stoppableObject.SwitchOnOrOff;
    }

    public void RemoveObject(StoppableObject stoppableObject)
    {
        myMultiDelegate -= stoppableObject.SwitchOnOrOff;
    }

    public void AddObject(VisibleChangeObject visibleChangeObject)
    {
        visibleChange += visibleChangeObject.VisibleChange;
    }

    public void RemoveObject(VisibleChangeObject visibleChangeObject)
    {
        visibleChange -= visibleChangeObject.VisibleChange;
    }
	
    // On or off function
    public void StartOrStopAllObjects(bool value)
    {
        if (myMultiDelegate != null) myMultiDelegate(value);
        if (visibleChange != null) visibleChange();
    }
}
