using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour
{
	void Start()
	{
		GetComponent<BinaryCharacterSaver>().LoadStartGameMenu();
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
