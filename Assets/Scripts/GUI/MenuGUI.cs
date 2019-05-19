using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGUI : BaseMenu
{
	public GameObject controller;
	public Slider progressBar;
	public GameObject optionPanel;

	private CameraToon toon;	// toon shader
	private MovableCamera movableCamera;	// Switch camera mode
	private Transform player;	// Get player position
	private bool open;	// menu is active or not

	private bool MenuOpenned() { return open && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1; }	// openning or opened
	private bool MenuClosed() { return !gameObject.activeInHierarchy; }	// closing or closed
	private bool MenuOpenningOrClosing() { return !MenuClosed() && !MenuOpenned(); }

	public void OpenOrCloseMenuButton()
	{
		// Initialize toon
		if (toon == null)
		{
			GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
			toon = camera.GetComponent<CameraToon>();
			movableCamera = camera.GetComponent<MovableCamera>();
			player = GameObject.FindGameObjectWithTag("Player").transform;

			animator = GetComponent<Animator>();	// In base menu class
		}

		// Throw if shader running or menu opening/closing
		if (toon.GetOn() || MenuOpenningOrClosing())
		{
			return;
		}

		open = !open;	// switch open state
		if (open) gameObject.SetActive(true);	// if inactive active first
		StartCoroutine("OpenOrCloseMenuAction", false);	// open or close menu		
	}

	public void CheckPointButton()
	{
		base.RetryButton();
	}

	public override void RetryButton()
	{
		// Update data
		BinaryCharacterSaver saver = GameObject.FindObjectOfType(typeof(BinaryCharacterSaver)) as BinaryCharacterSaver;
		saver.ClearCheckpoint();

		// base function
		base.RetryButton();
	}

	protected override IEnumerator RetryAction()
	{
		// Close Menu
		open = false;	// switch open state
		yield return StartCoroutine("OpenOrCloseMenuAction", true);	// open or close menu		

        // Turn on shader
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

        // Load scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	protected override IEnumerator LevelSelectAction()
	{		
		// Close Menu
		open = false;	// switch open state
		yield return StartCoroutine("OpenOrCloseMenuAction", true);	// open or close menu

        // Turn on shader
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

        // Load scene again
        SceneManager.LoadScene("LevelSelect");
	}

	public void SetOptionPanelActive(bool value)
	{
		if (optionPanel.activeInHierarchy != value)	// current status need to change
		{
			optionPanel.SetActive(value);
			movableCamera.SwitchCameraViewMode();
		}
	}

	private IEnumerator OpenOrCloseMenuAction(bool activeWhenClose)
	{		
		if (open)
		{
			// Set time scale to 0
			Time.timeScale = 0;
			
			// Disable player button
			controller.SetActive(false);

			// Camera cel shader
			toon.TurnOnOrOffShader();

			// Update current progress
			progressBar.value = player.position.x;

			// Fade in menu
			Open();

			// Wait for enable shader and menu fade in
			// while(animator.GetCurrentAnimatorClipInfo(0).Length != 0)
			while(toon.GetOn() || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
			{
				yield return null;
			}

			// Camera movable
			movableCamera.SwitchCameraViewMode();
		}
		else	// close
		{
			// Move camera back to player
			if (movableCamera.IsMapViewMode())
			{
				movableCamera.SwitchCameraViewMode();
			}

			// Wait for camera move back to player
			yield return StartCoroutine(movableCamera.MoveToPlayer());
		
			// Disable camera cel shader
			toon.TurnOnOrOffShader();

			// Fade out menu
			if (optionPanel.activeInHierarchy) optionPanel.SetActive(false);	// Close option button if active
			Close();

			// Wait for disable shader and menu fade out
			while(toon.GetOn() || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
			{
				yield return null;
			}

			// Enable player button
			controller.SetActive(true);

			// Set time scale to original
			Time.timeScale = 1;

			// Hide menu
			gameObject.SetActive(activeWhenClose);
		}
	}

}
