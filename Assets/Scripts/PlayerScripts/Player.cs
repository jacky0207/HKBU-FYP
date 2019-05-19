using UnityEngine;
using System.Collections;
// using UnityEngine.SceneManagement;

public enum Direction
{
    none = 0,
    left = -1,
    right = 1
}

[RequireComponent(typeof(Bag), typeof(DieAnimation))]
public class Player : MonoBehaviour
{
    private CameraGrey cameraGrey;

    private StoppableObjectManager stoppableObjectManager;
    private TimeGUI timeGUI;

    // private CameraPixelation cameraPixelation;


    private SpriteRenderer spriteRenderer;
	private Rigidbody2D rb;
    private Animator animator;

    private Bag bag;

    // Sub stage
    private MovableCamera movableCamera;
    private CountDown countDown;
    private bool inSubStage;
    private bool translating;

    private float gravity;
    private BoxCollider2D boxCollider2D;
    private float sceneHeight;

    private SoundManager soundManager;

    // Move
    public bool TestInMac;
    public Direction direction = Direction.none;
    public float moveSpeed = 1f * 3;
    private float fallSpeed;
	private bool moving;

    private float moveAudioPeriod = 0.4f;   // Sound
    private float moveAudioLastPlay;

	// Jump
    public float jumpSpeed = 3.5f * 1.8f;
    private bool falling = true;

    // Win / Bonus
    private bool canOpenDoor;
    private bool canOpenRedDoor;
    // private Transform translatePosition;
    private Goal translationPoint;

    // Die
    private bool dead;
    // public bool damageEnabled;

    // Win
    private bool win;

    void Awake()
    {
        // Time button
        cameraGrey = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraGrey>(); // shader
        stoppableObjectManager = GameObject.FindGameObjectWithTag("StoppableObjectManager").GetComponent<StoppableObjectManager>(); // manager        
        timeGUI = Resources.FindObjectsOfTypeAll<TimeGUI>()[0] as TimeGUI;
        
        // Animation
        spriteRenderer = GetComponent<SpriteRenderer>();    // Sprite flip X        
		rb = GetComponent<Rigidbody2D>();   // physics
        animator = GetComponent<Animator>();    // Animator

        // Item collection
        bag = GetComponent<Bag>();

        // Bonus room
        movableCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MovableCamera>();
        countDown = Resources.FindObjectsOfTypeAll<CountDown>()[0] as CountDown;

        // Falling
        gravity = Physics.gravity.y;    // fall force
        boxCollider2D = GetComponent<BoxCollider2D>();   // raycast width
        sceneHeight = GameObject.FindGameObjectWithTag("OuterBorder").GetComponent<OuterBorder>().BorderHeight();

        // Sound play
        // soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        soundManager = GetComponent<SoundManager>();
    }

    void MacPlatformTest()
    {
        bool left = Input.GetKey("a");
        bool right = Input.GetKey("d");
        bool timeFreeze = Input.GetKeyDown("j");
        bool jump = Input.GetKeyDown("k");

        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            if ((left && right) || (!left && !right))
            {
                direction = Direction.none;
            }
            else if (left)
            {
                direction = Direction.left;
            }
            else if (right)
            {
                direction = Direction.right;
            }
            
            if (timeFreeze)
            {
                TimeStartOrStop();
            }
            
            if (jump)
            {
                Jump();
            }
        }
    }

    void Update()
    {
        CheckGround();
		Move();

        if (TestInMac)
        {
            MacPlatformTest();
        }
    }

    private void CheckGround()
    {
        int groundBitMask = 8;
        
        // Raycast parameters
        Vector2 position = transform.position;
        Vector2 direction = Vector3.down;
        float distance = sceneHeight;
        int layerMask = 1 << groundBitMask; // Bit shift the index of the layer (8) to get a bit mask

        // bound position
        float deltaX = boxCollider2D.bounds.extents.x;
        Vector2 leftPosition = position + Vector2.left * deltaX;
        Vector2 rightPosition = position + Vector2.right * deltaX;

        // left and right bound raycast
        // out hit
        RaycastHit2D lefthit = Physics2D.Raycast(leftPosition,
                                                direction, 
                                                distance, 
                                                layerMask);

        RaycastHit2D righthit = Physics2D.Raycast(rightPosition,
                                                direction, 
                                                distance, 
                                                layerMask);
            
        // Debug raycast
        Debug.DrawRay(leftPosition, direction * lefthit.distance, Color.yellow);
        Debug.DrawRay(rightPosition, direction * righthit.distance, Color.yellow);

        // Check distance
        float groundDistance = 0.05f;    // Can't be 0

        // Falling case
        // Check state and velocity
        if (falling && fallSpeed < 0)
        {
            // Any one raycast ground
            if ((lefthit.collider != null && lefthit.distance < groundDistance) || 
                (righthit.collider != null && righthit.distance < groundDistance))
            {
                // set on ground
                FallingState(false);
            }
        }
        // Ground case
        else if (!falling)
        {
            // Two raycast leave
            if ((lefthit.collider != null && lefthit.distance >= groundDistance) &&
                (righthit.collider != null && righthit.distance >= groundDistance))
            {
                // set falling
                falling = true;
            }
        }
    }

    public void Move()
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

		// Throw idle
		if (direction == Direction.none)
		{
            Idle();
		}
        else
        {
            Run();
        }
    }

    private void Idle()
    {
        // Set animator move false if moving
        if (moving)
        {
            MovingState(false);
        }
        MovingUpdate(false, 0);
    }

    private void Run()
    {
        // Set animator move true if idling
        if (!moving)
        {
            MovingState(true);
        }
        MovingUpdate(direction == Direction.left, (int)direction * moveSpeed);

        // Sound
        if (!falling && Time.time > moveAudioLastPlay + moveAudioPeriod)
        {
            moveAudioLastPlay = Time.time;  // reset

            soundManager.PlayOnce("run");
        }
    }

    private void MovingState(bool value)
    {
        moving = value;
        animator.SetBool("move", value);
    }

    private void MovingUpdate(bool flip, float moveSpeedX)
    {        
        // Set sprite renderer flip
        spriteRenderer.flipX = flip;
        // Set rigidbody position
        fallSpeed = falling ? fallSpeed + gravity * Time.deltaTime : 0;
        rb.velocity = new Vector2(moveSpeedX, fallSpeed);
    }

    public void Jump()
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

        // Goal
        if (canOpenDoor)
        {
            // Win
            // StartCoroutine(Win());
            Win();
            
            // Sound
            // soundManager.playAudioOnce(4);
        }
        // Bonus
        else if (canOpenRedDoor)
        {
            // Translate after camera turn black;
            StartCoroutine(TranslateToRoom());

            // Sound
            // soundManager.PlayOnce("jump");
        }
        // jump
        else
        {
            if (falling)
            {
                return;
            }
            FallingState(true);
            // Set rigidbody velocity
            fallSpeed = jumpSpeed;
            rb.velocity = transform.up * jumpSpeed;

            // Sound
            soundManager.PlayOnce("jump");
        }
    }

    private IEnumerator TranslateToRoom()
    {
        // Set immutable
        translating = true;

        // Activate/Disactivate control of count down gui
        inSubStage = !inSubStage;

        // Stop count down if freezing
        bool on = cameraGrey.GetOn();

        if (on)
        {
            countDown.SetOn(false);
        }

        // Trigger camera animation
        movableCamera.GetComponent<CameraToBlack>().on = true;

        // Set door used
        translationPoint.DisableDoor();
        
        yield return new WaitForSeconds(0.25f);

        // Translate to other scene
        // transform.position = translatePosition.position;
        transform.position = translationPoint.translatePosition.position;

        // Camera set border
        movableCamera.SetBorder(translationPoint.cameraSource, translationPoint.cameraDestination);
        
        // Set immutable
        translating = false;
    }

    private void FallingState(bool value)
    {
        // Set falling
		falling = value;
        // Set animator here
        animator.SetBool("jump", value);
    }

    public void Die()
    {
        if (!dead)  // Not die yet
        {   
            dead = true;    // Will not trigger again
            
            // Stop time count
            timeGUI.SetOn(false);

            StartCoroutine(DieAction()); 
        }
    }

    // private void DieShader()
    private IEnumerator DieAction()
    {
        // Stop any animation
        animator.enabled = false;
        
        // Play die animation
        // GetComponent<DieAnimation>().on = true;
        yield return StartCoroutine(GetComponent<DieAnimation>().PlayAnimation());

        // Load die menu
        // Code here
        DieMenu[] dieMenus = Resources.FindObjectsOfTypeAll<DieMenu>() as DieMenu[];
        dieMenus[0].Open();

        // // Turn on shader
        // // GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().TurnOnOrOffShader();   // Shader
        // yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

        // // Load scene again
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // private IEnumerator Win()
    private void Win()
    {
        if (!win)
        {
            win = true;

            // Stop time count
            timeGUI.SetOn(false);
            
            // Load win menu
            // Code here
            WinMenu[] winMenus = Resources.FindObjectsOfTypeAll<WinMenu>() as WinMenu[];
            winMenus[0].Open();

            // // Turn on shader
            // yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());
        
            // // Load next scene
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TimeStartOrStop()
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

        cameraGrey.TurnOnOrOffShader();   // Turn on the camera shader

        bool on = cameraGrey.GetOn();
        
        // Stop all other gameobject
        stoppableObjectManager.StartOrStopAllObjects(!on);

        // Stop time count
        timeGUI.SetOn(!on);

        // Stop count down
        if (inSubStage)
        {
            countDown.SetOn(!on);
        }
        
        // Sound
        soundManager.PlayOnce("time");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

        // // Reach destination
        // if (col.gameObject.tag == "EndPoint")
        // {
        //     StartCoroutine(Win());
        // }

        // Die but check whether damage when stop
        if (col.gameObject.tag == "Damagable")
        {
            StoppableObject stoppableObject = col.gameObject.GetComponent<StoppableObject>();
            // Die if not stoppable object or always damagable
            if (stoppableObject == null || !cameraGrey.GetOn() || (cameraGrey.GetOn() && stoppableObject.freezeDamagable))
            {
                Die();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

        // Stop jump
        // if (col.gameObject.tag == "Ground")
		// {
		//     if (falling)
		// 	{
		// 		FallingState(false);
		// 	}
		// }

        // Get key
        if (col.gameObject.tag == "Key")
        {
            bag.AddItem(col.gameObject.GetComponent<Key>());            

            // Sound
            soundManager.PlayOnce("get");
        }
        
        // Get jewellary
        if (col.gameObject.tag == "Jewellary")
        {
            bag.AddItem(col.gameObject.GetComponent<Jewellary>());
            
            // Sound
            soundManager.PlayOnce("get");
        }

        // Checkpoint
        if (col.gameObject.tag == "Checkpoint")
        {
            BinaryCharacterSaver saver = GameObject.FindObjectOfType(typeof(BinaryCharacterSaver)) as BinaryCharacterSaver;
            saver.Checkpoint(col.gameObject.transform.GetSiblingIndex(), bag);
            // Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            
            // Sound
            soundManager.PlayOnce("get");
        }

        // Reach destination
        if (col.gameObject.tag == "EndPoint")
        {
            CollideWithGoal(col, true);
        }

        // Reach red room
        if (col.gameObject.tag == "RedRoom")
        {
            CollideWithRedDoor(col, true);
        }

        // Die directly
        if (col.gameObject.tag == "Damagable")
        {
            Die();
        }
    }

    // void OnTriggerStay2D(Collider2D col)
    // {
    //     // Stop jump
    //     if (col.gameObject.tag == "Ground")
	// 	{
	// 	    if (falling)
	// 		{
	// 			FallingState(false);
	// 		}
	// 	}
    // }

    void OnTriggerExit2D(Collider2D col)
    {
        // Can't called when win or die
        if (win || dead || translating)
        {
            return;
        }

        // // Stop jump
        // if (col.gameObject.tag == "Ground")
		// {
		//     if (!falling)
		// 	{
		// 		falling = true;
		// 	}
		// }

        // Leave destination
        if (col.gameObject.tag == "EndPoint")
        {
            CollideWithGoal(col, false);
        }

        // Leave red room
        if (col.gameObject.tag == "RedRoom")
        {
            CollideWithRedDoor(col, false);
        }
    }

    private void CollideWithGoal(Collider2D col, bool collide)
    {
        if (collide)
        {
            canOpenDoor = col.gameObject.GetComponent<Goal>().CanOpen(bag);
        }
        else
        {
            canOpenDoor = false;
            col.gameObject.GetComponent<Goal>().LeaveDoor();
        }
    }

    private void CollideWithRedDoor(Collider2D col, bool collide)
    {
        if (collide)
        {
            canOpenRedDoor = col.gameObject.GetComponent<Goal>().CanOpen(bag);
            // translatePosition = col.gameObject.GetComponent<Goal>().translatePosition;
            translationPoint = col.gameObject.GetComponent<Goal>();
        }
        else
        {
            canOpenRedDoor = false;
            // translatePosition = null;
            translationPoint = null;
            col.gameObject.GetComponent<Goal>().LeaveDoor();
        }
    }

}