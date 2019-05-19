using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BinaryCharacterSaver))]
public class WinMenu : BaseMenu
{
	[Header("Time")]
	public Text timeText;
	
	[Header("Jewellery")]
	public Sprite greyDiamondSprite;
	public Image redDiamondIcon, greenDiamondIcon, blueDiamondIcon;
	public Text redDiamondText, greenDiamondText, blueDiamondText;

	[Header("Final Score")]
	public Text scoreText;
	public GameObject buttons;
	private int finalScore;	// time score + jewellery score

	public void UpdateGUI()
	{
		// Time
        TimeGUI timeGUI = Resources.FindObjectsOfTypeAll<TimeGUI>()[0] as TimeGUI;
		double time = System.Math.Round(timeGUI.GetTime(), 2);
		int idealTime = timeGUI.GetIdealTime();

		timeText.text = time + "s";

		int timeScore = idealTime * 1000; 	// 1st time category
		timeScore -= (int) ((time - idealTime) * 1000);
		// time: 0s, 60s, 90s, 120s
		// score: 120000, 60000, 30000, 0

		// Jewellery
        Bag bag = Resources.FindObjectsOfTypeAll<Bag>()[0] as Bag;
		Dictionary<ItemColor, int> jewellerys = bag.GetAllJewellarys();
		ItemColor[] colors = ItemColor.GetValues(typeof(ItemColor)) as ItemColor[];

		DisableJewelleryIcon(jewellerys);
		int jewelleryScore = GetJewelleryScore(jewellerys);	// 2nd time category
		// score: 5000, 10000, 15000

		// Final score
		finalScore = timeScore + jewelleryScore;

		// Update data
		GetComponent<BinaryCharacterSaver>().StageClear(finalScore);

		// int scoreAppendDigit = GetScoreAppendDigit(finalScore, 6);	// 000000		
		// string scoreAppend = GetScoreAppendString(scoreAppendDigit);

		// scoreText.text = scoreAppend + finalScore;

		// buttons.SetActive(true);
	}

	private void DisableJewelleryIcon(Dictionary<ItemColor, int> jewellerys)
	{
		// Change to grey if not collect
		if (!jewellerys.ContainsKey(ItemColor.red))
		{
			redDiamondIcon.sprite = greyDiamondSprite;
			redDiamondText.text = "";
		}

		if (!jewellerys.ContainsKey(ItemColor.green))
		{
			greenDiamondIcon.sprite = greyDiamondSprite;
			greenDiamondText.text = "";
		}

		if (!jewellerys.ContainsKey(ItemColor.blue))
		{
			blueDiamondIcon.sprite = greyDiamondSprite;
			blueDiamondText.text = "";
		}
	}

	private int GetJewelleryScore(Dictionary<ItemColor, int> jewellerys)
	{
		int jewelleryScore = 0;

		// Add score if collect
		if (jewellerys.ContainsKey(ItemColor.red))
		{
			jewelleryScore += jewellerys[ItemColor.red];
		}

		if (jewellerys.ContainsKey(ItemColor.green))
		{
			jewelleryScore += jewellerys[ItemColor.green];
		}

		if (jewellerys.ContainsKey(ItemColor.blue))
		{
			jewelleryScore += jewellerys[ItemColor.blue];
		}

		return jewelleryScore;
	}

	private int GetScoreAppendDigit(int score, int digit)
	{
		while(score/10 != 0)
		{
			digit -= 1;
			score /= 10;
		}
		return digit;
	}

	private string GetScoreAppendString(int digit)
	{
		string scoreAppend = "";
		for (int count = 1; count < digit; count++)
		{
			scoreAppend += "0";
		}
		return scoreAppend;
	}

	public void CalculateScore()
	{
		StartCoroutine(UpdateScore());
	}

	private IEnumerator UpdateScore()
	{		
		float countDeltaTime = 0.01f;
		int increment = (int) (finalScore * countDeltaTime);

		int counter = 0;
		while(counter < finalScore)
		{
			counter += increment;

			if (counter > finalScore)
			{
				counter = finalScore;
				buttons.SetActive(true);
				// Destroy(buttons);
			}

			int scoreAppendDigit = GetScoreAppendDigit(counter, 6);	// 000000		
			string scoreAppend = GetScoreAppendString(scoreAppendDigit);
			scoreText.text = scoreAppend + counter;
			yield return new WaitForSeconds(countDeltaTime);
		}
	}

	// // Next Stage
	// public void NextStageButton()
	// {
	// 	// Animation
	// 	StartCoroutine(NextStageAction());
	// }

	// public IEnumerator NextStageAction()
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
}
