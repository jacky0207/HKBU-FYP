using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieMenu : BaseMenu
{
	// // Check point
	// public void CheckPointButton()
	// {
	// 	// Animation
	// 	StartCoroutine(CheckPointAction());
	// }

	// public IEnumerator CheckPointAction()
	// {
	// 	// Close panel
	// 	Close();

	// 	// Wait for animation
	// 	while(animationComponent.isPlaying)
	// 	{
	// 		yield return null;
	// 	}

    //     // Turn on shader
    //     yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

    //     // Load scene again
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	// }

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
}
