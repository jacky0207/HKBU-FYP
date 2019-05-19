using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTVTriggerState : TriggerState
{
	// Overlap checking
	private Collider2D playerCollider;	// player collider
	private Collider2D detecter;	// fsm object collider

	// Overlap action
	private GameObject shootSignal;
	private SpriteRenderer shootSignalSpriteRenderer;
	private float fadeOutCounter;	// count shoot signal fade out time
	private float fadeOutWaitTime = 0.1f;	// count shoot signal fade out time

	// sound
	private AudioSource audioSource;

	public CCTVTriggerState(FiniteStateMachine parent, float waitTime, int frequency) : base(parent, waitTime, frequency)
	{
		playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
		detecter = parent.GetParent().GetComponent<Collider2D>();

		shootSignal = detecter.transform.GetChild(0).gameObject;
		shootSignalSpriteRenderer = shootSignal.GetComponent<SpriteRenderer>();
		
		audioSource = parent.GetParent().GetComponent<AudioSource>();
	}
	
	public override void Update()
	{
		// Invoke after wait time instead of immediately start
		if (counter == 9999)
		{
			counter = 0;
		}
		
		// Do action
		if (times < frequency)
		{
			// Action waiting
			if (counter < waitTime)
			{
				counter += Time.deltaTime;
		
				// Enable shoot signal
				if (!shootSignal.activeInHierarchy)
				{
					shootSignal.SetActive(true);
				}

				// Fade in
				float fadeInCounter = counter - (waitTime - fadeOutWaitTime);
				if (fadeInCounter < fadeOutWaitTime)
				{
					// Edit to original
					float scale = fadeInCounter / fadeOutWaitTime;
					shootSignal.transform.localScale = Vector3.one * 0.75f + Vector3.one * (Mathf.Clamp(scale * 0.25f, 0, 1));
					shootSignalSpriteRenderer.color = new Color(scale, 0, 0);
				}
				// // Edit shoot signal
				// float scale = counter / waitTime;
				// shootSignal.transform.localScale = Vector3.one * 0.75f + Vector3.one * (Mathf.Clamp(scale * 0.25f, 0, 1));
				// shootSignalSpriteRenderer.color = new Color(scale, 0, 0);
			}
			// Next action
			else
			{
				// Code here
				Action1();
				
				// Reset variable
				ActionSetVariable();
			}
		}
		// Finish
		else
		{
			// Fade out
			if (fadeOutCounter < fadeOutWaitTime)
			{
				fadeOutCounter += Time.deltaTime;
				
				// Edit to original
				float scale = fadeOutCounter / fadeOutWaitTime;			
				shootSignal.transform.localScale = Vector3.one - Vector3.one * (Mathf.Clamp(scale * 0.25f, 0, 1));
				shootSignalSpriteRenderer.color = new Color(1 - scale, 0, 0);
			}
			// end
			else
			{
				// Correct and Disable shoot signal
				shootSignal.transform.localScale = Vector3.one * 0.75f;
				shootSignalSpriteRenderer.color = Color.black;
				shootSignal.SetActive(false);

				// To other state 
				Transition1();
			}

		}
	}
		
	protected override void Action1()
	{
		// CCTV attack player
		if (playerCollider.bounds.Intersects(detecter.bounds))
		{
			// call player die
			playerCollider.GetComponent<Player>().Die();
		}

		// Edit shoot signal
		shootSignal.transform.localScale = Vector3.one;
		shootSignalSpriteRenderer.color = Color.red;

		// sound
		// detecter.GetComponent<SoundManager>().PlayOnce("shot");
		audioSource.Play();
	}

	protected override void Transition1()
	{
		// Reset variable
		fadeOutCounter = 0;

		Transition(FSMState.Patrol);
	}
}
