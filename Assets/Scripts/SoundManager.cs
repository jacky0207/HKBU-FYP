using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
	[System.Serializable]
	public class Clip
	{
		public string name;
		public AudioClip obj;
	}

	public Clip[] clips;
	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void PlayOnce(string name)
	{
		AudioClip audioClip = GetAudioClip(name);

		if (!audioClip)
		{
			Debug.LogWarning("Usage: Clip not found");
			return;
		}

		audioSource.PlayOneShot(audioClip);
	}

	private AudioClip GetAudioClip(string name)
	{
		// Loop to find clip
		foreach (var clip in clips)
		{
			// Check name equivalent
			if (clip.name.Equals(name))
			{
				return clip.obj;
			}
		}

		// Cannot find clip
		return null;
	}

}