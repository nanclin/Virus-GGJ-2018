using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;


	public static GameManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void RestartGame()
	{
		UiController.Instance.OpenMainUi();
	}

	public void EndGame()
	{
		UiController.Instance.OpenPostmatch();
	}
}
