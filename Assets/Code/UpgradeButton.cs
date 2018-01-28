using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
	public GameObject flask;
	public Text priceText;
	public GameObject[] points;
	public UpgradeType upgradeType;

	public AudioClip buttonSfx;
	private void OnEnable()
	{
		SetPoints(ZombieStats.Instance.GetUpgradeLevel(upgradeType));
		SetPrice(ZombieStats.Instance.GetUpgradePrice(upgradeType));
	}

	public void OnUpgrade()
	{
		if(GameManager.Instance.GetAvailableSkillPoints() < ZombieStats.Instance.GetUpgradePrice(upgradeType))
		{
			return;
		}

		GameManager.Instance.audioSource.PlayOneShot(buttonSfx);
		GameManager.Instance.AddSkillPoints(-ZombieStats.Instance.GetUpgradePrice(upgradeType));
		ZombieStats.Instance.UpgradeSkill(upgradeType);
		int level = ZombieStats.Instance.GetUpgradeLevel(upgradeType);
		SetPoints(ZombieStats.Instance.GetUpgradeLevel(upgradeType));

		if(level < 5)
		{
			SetPrice(ZombieStats.Instance.GetUpgradePrice(upgradeType));
		}
		else
		{
			flask.SetActive(false);
			priceText.gameObject.SetActive(false);
		}

	}

	private void SetPoints(int pointsCount)
	{
		for(int i = 0; i < points.Length; i++)
		{
			points[i].SetActive(i < pointsCount);
		}

	}

	private void SetPrice(int price)
	{
		priceText.text = price.ToString();
	}
}
