using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnPoint : StoppableObject
{
	public float idleWaitTime = 3.0f;
    public float triggerWaitTime = 1.0f;
	public int frequency = 1;

    public GameObject bullet;
    private GameObject[] bullets = new GameObject[3];
	public Transform spawnPoint;
    public Transform leftbound;
    public Transform rightbound;

    #region start
    
    protected override void InitializeFSM()
    {
        fsm = new BulletSpawnPointFSM(this);
        bullets = new GameObject[frequency];
    }

    protected override void AddFSMStates()
    {
        fsm.Idle(idleWaitTime).Trigger(triggerWaitTime, frequency);
    }

    #endregion

	public void Spawn(int index)
	{
        // Use fall spike
        if (bullets[index] == null)
        {
		    bullets[index] = Instantiate(bullet, transform.position, transform.rotation, spawnPoint);

            // Bound
            if (leftbound != null)
            {
                bullets[index].GetComponent<Bullet>().SetLeftBound(leftbound);
            }
            if (rightbound != null)
            {
                bullets[index].GetComponent<Bullet>().SetRightBound(rightbound);
            }
        }
        else
        {
            spawnPoint.GetChild(index).gameObject.SetActive(true);
        }
	}
}
