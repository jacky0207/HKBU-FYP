using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleChangeObject : MonoBehaviour
{
    public bool freezeRenderChange;
	public bool VisibleAfterIntersect;
    protected StoppableObjectManager stoppableObjectManager;

	protected Transform player;
	// private Collider2D objectCollider;
    protected Collider2D detecter;

	private Collider2D playerCollider;
	private bool collided;


    protected virtual void OnEnable()
    {
        if (stoppableObjectManager == null)
        {        
            stoppableObjectManager = GameObject.FindGameObjectWithTag("StoppableObjectManager").GetComponent<StoppableObjectManager>();
        }
        stoppableObjectManager.AddObject(this);

        // Get Player
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

		// playerCollider = player.gameObject.GetComponent<Collider2D>();

        EnableSetting();    // Custom
    }

    private void OnDisable()
    {
        stoppableObjectManager.RemoveObject(this);
        DisableSetting();    // Custom
    }

    protected virtual void EnableSetting() {}
    protected virtual void DisableSetting() {}

	protected virtual void Start()
	{
		detecter = GetComponent<Collider2D>();
		playerCollider = player.gameObject.GetComponent<Collider2D>();
	}

	protected virtual void Update()
	{
		if (VisibleAfterIntersect)
		{
			CheckIntersectWithPlayer();
		}
	}

	private void CheckIntersectWithPlayer()
	{
		if (!collided)
		{
			if (playerCollider.bounds.Intersects(detecter.bounds))
			{
				collided = true;
				Color color = GetComponent<SpriteRenderer>().color;
				GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0.5f);
			}
		}
	}

	public void VisibleChange()
	{		
        if (freezeRenderChange) // invisible changed
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
        }
	}

}