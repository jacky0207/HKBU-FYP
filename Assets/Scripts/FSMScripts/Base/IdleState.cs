using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	private float waitTime;
	private float counter;

	public IdleState(FiniteStateMachine parent, float waitTime) : base(parent)
	{
		this.waitTime = waitTime;
	}
	
	public override void Update ()
	{
		counter += Time.deltaTime;
 
  		if (counter >= waitTime)
 		{
    		Transition1();
 		}

		// parent.GetParent().StartCoroutine("DoCoroutine", UpdateFunction());
	}

	// IEnumerator UpdateFunction()
    // {
    //     yield return new WaitForSeconds(waitTime);
		// 	parent.GetParent().StopCoroutine("DoCoroutine");
		// 	Transition1();
    // }

	protected virtual void Transition1()
	{
		Transition(FSMState.Disable);
	}

	protected override void TransitionResetVariable()
	{
		counter = 0;
	}

    // public override void OnTriggerEnter2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         parent.collideWithPlayer = true;
    //     }
    // }

    // public override void OnTriggerExit2D(Collider2D col)
    // {
    //     // Collide player
    //     if (col.gameObject.tag == "Player")
    //     {
    //         parent.collideWithPlayer = false;
    //     }
    // }

}