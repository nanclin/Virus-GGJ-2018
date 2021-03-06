﻿using System;
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

	public void Reset()
	{
		upgradeLevelByUpgradeType = new int[Enum.GetNames(typeof(UpgradeType)).Length];
	}

	public int GetUpgradeLevel(UpgradeType upgradeType)
	{
		return upgradeLevelByUpgradeType[(int)upgradeType];
	}

	public void UpgradeSkill(UpgradeType upgradeType)
	{
		upgradeLevelByUpgradeType[(int)upgradeType]++;
		upgradeLevelByUpgradeType[(int)upgradeType] = Mathf.Min(upgradeLevelByUpgradeType[(int)upgradeType], 5);
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
			Debug.Log("ASD: " + GetUpgradeLevel(UpgradeType.Damage));
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


}

public enum UpgradeType
{
	MovementSpeed,
	Damage,
	Radius,
	Lifetime
}