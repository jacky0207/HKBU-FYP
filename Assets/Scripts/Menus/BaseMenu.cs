using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public abstract class BaseMenu : MonoBehaviour
{
	// protected Animation animationComponent;
	protected Animator animator;

	public void Open()
	{
		// Get animation component
		// animationComponent = GetComponent<Animation>();
		animator = GetComponent<Animator>();

		// Animation
		// animationComponent.Play("Open");
		animator.SetTrigger("Open");
	}

	protected void Close()
	{
		// Animation
		// animationComponent.Play("Close");
		animator.SetTrigger("Close");
	}

	// Retry
	public virtual void RetryButton()
	{
		// Animation
		StartCoroutine(RetryAction());
	}

	protected virtual IEnumerator RetryAction()
	{
		// Close panel]
		Close();

		// Wait for animation
		// while(animationComponent.isPlaying)
		while(animator.GetCurrentAnimatorClipInfo(0).Length != 0)
		{
			yield return null;
		}

        // Turn on shader
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

        // Load scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// Level select
	public void LevelSelectButton()
	{
		// Animation
		StartCoroutine(LevelSelectAction());
	}

	protected virtual IEnumerator LevelSelectAction()
	{
		// Close panel]
		Close();

		// Wait for animation
		// while(animationComponent.isPlaying)
		while(animator.GetCurrentAnimatorClipInfo(0).Length != 0)
		{
			yield return null;
		}

        // Turn on shader
        yield return StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraPixelation>().PlayAnimation());

        // Load scene again
        SceneManager.LoadScene("LevelSelect");
	}
}
