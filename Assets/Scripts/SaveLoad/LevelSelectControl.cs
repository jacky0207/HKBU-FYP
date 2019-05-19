using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectControl : MonoBehaviour
{
	void Start()
	{
		GetComponent<BinaryCharacterSaver>().LoadLevelSelectMenu();
	}
	
	public void GoToScene(string stage)
	{
		GetComponent<BinaryCharacterSaver>().ClearCheckpoint();
		SceneManager.LoadScene(stage);
	}
}
