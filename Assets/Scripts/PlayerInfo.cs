using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{

	/// <summary>
	/// ресурсы игрока
	/// </summary>
	internal PlayerResources Resources;

	private float playerHP;

	internal float PlayerHP
	{
		get
		{
			return playerHP;
		}
		set
		{
			playerHP = value;
			GameInfo.gameInfo.UpdateUI();
		}
		
	}
	
	/// <summary>
	/// цвет игрока
	/// </summary>
	internal String PlayerColor;

	/// <summary>
	/// Коэффициент на который умножается вся добыча игрока
	/// </summary>
	public float IncomeCoefficient = 1f; 
	
	/// <summary>
	///Можно ли разместить юнита на базе этого игрока 
	/// </summary>
	internal bool IsBaseBlocked;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
