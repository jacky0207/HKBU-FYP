using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpikeSpawnPoint : StoppableObject
{
    public float idleWaitTime = 3.0f;
    public float triggerWaitTime = 1.0f;
	public int frequency = 1;

    public GameObject fallSpike;
    private List<GameObject> fallSpikes = new List<GameObject>();

    private SoundManager soundManager;

    void Awake()
    {
        soundManager = GetComponent<SoundManager>();
    }

    protected override void InitializeFSM()
    {
        fsm = new FallSpikeSpawnPointFSM(this);
    }

    protected override void AddFSMStates()
    {
        fsm.Idle(idleWaitTime).Trigger(triggerWaitTime, frequency);
    }

	public void Spawn(int index)
	{
        // Use fall spike
        if (fallSpikes.Count < index + 1)
        {
		    fallSpikes.Add(Instantiate(fallSpike, transform.position, transform.rotation, transform));
        }
        else
        {
            transform.GetChild(index).gameObject.SetActive(true);
        }

        // sound
        soundManager.PlayOnce("spawn");
	}

}
