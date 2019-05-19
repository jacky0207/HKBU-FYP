using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoppableObject : VisibleChangeObject
{
    public bool freezeDamagable = false;
    // public bool freezeRenderChange = false;
    public bool useFSM = true;
    protected bool on = true;
    protected FiniteStateMachine fsm;

    // private StoppableObjectManager stoppableObjectManager;
    // private Collider2D detecter;
    private float sceneHeight;

	// protected Transform player;

    protected override void OnEnable()
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

        EnableSetting();    // Custom
    }

    protected override void Start()
    {
        base.Start();
        
        InitializeFSM();
        AddFSMStates();

        // detecter = GetComponent<Collider2D>();   // raycast width
        sceneHeight = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().BorderHeight();
    }

    protected virtual void InitializeFSM() { fsm = new FiniteStateMachine(this); }
    protected virtual void AddFSMStates() {}

    protected override void Update()
    {
        base.Update();
        
        if (!useFSM)
        {
            return;
        }
        
        if (on)
        {
            fsm.Update();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!useFSM)
        {
            return;
        }

        fsm.OnTriggerEnter2D(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!useFSM)
        {
            return;
        }
        
        fsm.OnTriggerStay2D(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!useFSM)
        {
            return;
        }
        
        fsm.OnTriggerExit2D(col);
    }
	
	private void OnDrawGizmos()
	{
        if (!useFSM)
        {
            return;
        }

        if (!Application.isPlaying) 
        {
            return;
        }

		fsm.OnDrawGizmos();
	}


    public void SwitchOnOrOff(bool value)
    {
        on = value;

        // if (freezeRenderChange) // invisible changed
        // {
        //     GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled ;
        // }
        VisibleChange();
    }

    public void SetVisible(bool value)
    {
        GetComponent<SpriteRenderer>().enabled = value;
        GetComponent<Collider2D>().enabled = value;
    }

    public bool DetectPlayerAbove()
    {
        int playerBitMask = 9;
        
        // Raycast parameters
        Vector2 position = transform.position;
        Vector2 direction = Vector3.up;
        float distance = sceneHeight;
        int layerMask = 1 << playerBitMask; // Bit shift the index of the layer (8) to get a bit mask

        // bound position
        float halfX = detecter.bounds.extents.x;
        float halfY = detecter.bounds.extents.y;

        // Check distance
        float groundDistance = 0.05f;    // Can't be 0
        float rayCount = 10;    // Can't be 0

        // Generate 10 raycast to check
        for (int count = 0; count < rayCount; count++)
        {
            float deltaX = -halfX + 2 * halfX * count / rayCount;   // corresponsing position x to center.
            Vector2 leftPosition = position + new Vector2(deltaX, halfY);
            // left and right bound raycast
            // out hit
            RaycastHit2D hit = Physics2D.Raycast(leftPosition,
                                                direction, 
                                                distance, 
                                                layerMask);
                
            // Debug raycast
            Debug.DrawRay(leftPosition, direction * hit.distance, Color.yellow);

            // Check distance
            // Detect player and ground
            if (hit.collider != null && hit.distance < groundDistance)
            {
                return true;
            }
        }

        return false;
    }

	// public IEnumerator DoCoroutine(IEnumerator cor)
    // {
    //     while (cor.MoveNext())
    //     {
    //         yield return cor.Current;
    //     }
    // }

    public bool GetOn()
    {
        return on;
    }

}