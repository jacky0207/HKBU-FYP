using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneControl : MonoBehaviour
{
	void Start()
	{
		GetComponent<BinaryCharacterSaver>().LoadCheckpoint();
	}
	
	public void GoToScene(string stage)
	{
		SceneManager.LoadScene(stage);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
	
}
