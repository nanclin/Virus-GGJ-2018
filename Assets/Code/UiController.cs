
using System.Collections;
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

	public AudioClip gameover;

	public GameObject postmatchText2;

	private void Awake()
	{
		instance = this;
	}

	public void SetScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void SetSkillPoints(int skillPoints)
	{
		skillPointsText.text = skillPoints.ToString();
	}

	public void OpenMainUi()
	{
		mainUiGo.SetActive(true);
		postmatchGo.SetActive(false);
		postmatchText2.SetActive(false);
	}

	public void OpenPostmatch()
	{
		mainUiGo.SetActive(false);
		postmatchGo.SetActive(true);
		GameManager.Instance.audioSource.PlayOneShot(gameover);
		StartCoroutine(ShowDelayedPostmatchText());
	}

	private IEnumerator ShowDelayedPostmatchText()
	{
		yield return new WaitForSeconds(1);
		postmatchText2.SetActive(true);
	}
}