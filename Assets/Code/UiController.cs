using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
	private static UiController instance;

	public static UiController Instance
	{
		get
		{
			return instance;
		}
	}

	public Text scoreText;
	public Text skillPointsText;

	public GameObject mainUiGo;
	public GameObject postmatchGo;

	private void Awake()
	{
		instance = this;
	}

	public void SetScore(int score)
	{
		scoreText.text = "Score: " + score;
	}

	public void SetSkillPoints(int skillPoints)
	{
		skillPointsText.text = "Skill points: " + skillPoints;
	}

	public void OpenMainUi()
	{
		mainUiGo.SetActive(true);
		postmatchGo.SetActive(false);
	}

	public void OpenPostmatch()
	{
		mainUiGo.SetActive(false);
		postmatchGo.SetActive(true);
	}
}