using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : StoppableObject
{
	// Idle
	public float idleWaitTime = 0.5f;

	// Patrol
	public float moveSpeed = 2.0f;
	public float mass = 50.0f;
	public float curSpeed = 4.0f;
	public float reachRadius = 0.1f;
	// public float avoidDistance = 2.0f;
	// public float avoidForce = 20.0f;
	public float areaWidth = 19.2f;

    private SoundManager soundManager;	// sound

	// Attack
	public float attackWaitTime = 0.5f;

	void Awake()
	{
		soundManager = GetComponent<SoundManager>();
	}

    protected override void InitializeFSM()
    {
        fsm = new CCTVFSM(this);
    }

    protected override void AddFSMStates()
    {
        fsm
		.Idle(idleWaitTime)
		.Patrol(GetPatrolDataDictionary())
		.Trigger(attackWaitTime);
    }

	private Dictionary<string, float> GetPatrolDataDictionary()
	{
		Dictionary<string, float> data = new Dictionary<string, float>();

		data["moveSpeed"] = moveSpeed;
		data["mass"] = mass;
		data["curSpeed"] = curSpeed;
		data["reachRadius"] = reachRadius;
		data["areaWidth"] = areaWidth;
		// data["avoidDistance"] = avoidDistance;
		// data["avoidForce"] = avoidForce;

		return data;
	}

	// public void PlayShotSound()
	// {
	// 	soundManager.PlayOnce("shot");
	// }

}
