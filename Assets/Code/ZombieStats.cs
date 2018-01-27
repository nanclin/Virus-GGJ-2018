using System;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : MonoBehaviour
{
	private static ZombieStats instance;

	private int[] upgradeLevelByUpgradeType;

	[SerializeField] private float[] movementSpeedArray;
	[SerializeField] private int[] damageArray;
	[SerializeField] private int[] radiusArray;
	[SerializeField] private int[] lifetimeArray;
	[SerializeField] private int[] healthArray;

	[SerializeField] private int[] upgradePriceArray;

	public static ZombieStats Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		upgradeLevelByUpgradeType = new int[Enum.GetNames(typeof(UpgradeType)).Length];
	}

	private int GetUpgradeLevel(UpgradeType upgradeType)
	{
		return upgradeLevelByUpgradeType[(int)upgradeType];
	}

	public void UpgradeSkill(UpgradeType upgradeType)
	{
		upgradeLevelByUpgradeType[(int)upgradeType]++;
	}

	public int GetUpgradePrice(UpgradeType upgradeType)
	{
		return upgradePriceArray[GetUpgradeLevel(upgradeType)];
	}

	public float MovementSpeed
	{
		get
		{
			return movementSpeedArray[GetUpgradeLevel(UpgradeType.MovementSpeed)];
		}
	}

	public int Damage
	{
		get
		{
			return damageArray[GetUpgradeLevel(UpgradeType.Damage)];
		}
	}

	public int Radius
	{
		get
		{
			return radiusArray[GetUpgradeLevel(UpgradeType.Radius)];
		}
	}

	public int Lifetime
	{
		get
		{
			return lifetimeArray[GetUpgradeLevel(UpgradeType.Lifetime)];
		}
	}

	public int Health
	{
		get
		{
			return healthArray[GetUpgradeLevel(UpgradeType.Health)];
		}
	}


}

public enum UpgradeType
{
	MovementSpeed,
	Damage,
	Radius,
	Lifetime,
	Health
}