using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainScript : MonoBehaviour
{

	/// <summary>
	/// Время между волнами
	/// </summary>
	public float SpawnTime;
	
	/// <summary>
	/// Время когда башни не препятствуют столкновению
	/// </summary>
	public static float NonCollisionTime = 6f;
	
	public static float GamePeriodTime = 6f;
	
	/// <summary>
	/// Можно ли размещать юнитов
	/// </summary>
	public static bool EnableSpawning = true;
	
	/// <summary>
	/// Текст времени
	/// </summary>
	public Text txt;

	/// <summary>
	/// Сколько осталось времени до следубющей волны
	/// </summary>
	private int TimeLeft;
	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnUnits", SpawnTime, SpawnTime);
        TimeLeft = (int)SpawnTime;
        InvokeRepeating("UpdateTime",0,1);
	}

	
	/// <summary>
	/// Вызывает всех юнитов на всех базах
	/// </summary>
	void SpawnUnits()
	{
		
		if(!EnableSpawning) return;
		
		foreach (var dull in GameObject.FindGameObjectsWithTag("BlueDull"))
		{
			(dull.GetComponent("DullScript") as DullScript).Spawn();
		}
		foreach (var dull in GameObject.FindGameObjectsWithTag("RedDull"))
		{
			(dull.GetComponent("DullScript") as DullScript).Spawn();
		}
		
	}

	void UpdateTime()
	{
		txt.text = TimeLeft.ToString();
		TimeLeft = (TimeLeft - 1 + (int)SpawnTime) % ((int)SpawnTime);

		
		GameInfo.RedPlayer.Resources.AmountOfGold += 0.5f * GameInfo.RedPlayer.IncomeCoefficient;
		GameInfo.BluePlayer.Resources.AmountOfGold += 0.5f * GameInfo.BluePlayer.IncomeCoefficient;
	}
	
}
