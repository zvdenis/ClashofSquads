using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TowerScript : UnitScript
{
	/// <summary>
	/// Звук уничтожения
	/// </summary>
	public AudioClip DeathSound;
	
	/// <summary>
	/// Снаряд которым стреляет башня
	/// </summary>
	public GameObject projectile;

	/// <summary>
	/// Эффект уничтожения башги
	/// </summary>
	public GameObject DeathParticleSystem;
	
	/// <summary>
	/// Навигационный объект башни
	/// </summary>
	protected NavMeshAgent TowerAgent;
	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		InvokeRepeating("TurnOffCollision", 0f, TerrainScript.GamePeriodTime);
		InvokeRepeating("TurnOnCollision", TerrainScript.NonCollisionTime, TerrainScript.GamePeriodTime);


		if (ColorOfUnit == "Blue")
		{
			GameInfo.BluePlayer.PlayerHP = 1;
			OnHpChanged += delegate { GameInfo.BluePlayer.PlayerHP = currentHealth / HealthOfUnit; };
		}
		if (ColorOfUnit == "Red")
		{
			GameInfo.RedPlayer.PlayerHP = 1;
			OnHpChanged += delegate { GameInfo.RedPlayer.PlayerHP = currentHealth / HealthOfUnit; };
		}
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
	}

	/// <summary>
	/// Включить столкновение с башней
	/// </summary>
	protected void TurnOnCollision()
	{
		UnitAgent.radius = 1;
	}

	/// <summary>
	/// Выключить столкновение с башней
	/// </summary>
	protected void TurnOffCollision()
	{
		UnitAgent.radius = 0;
	}
	
	/// <summary>
	/// Атакует противника
	/// </summary>
	protected override void  AttackEnemy()
	{
		ArrowShot();
	}

	/// <summary>
	/// Смерть юнита, вызывает звуки и эффекты смерти
	/// </summary>
	protected override void UnitDied()
	{
		Debug.Log("Game finished " + ColorOfUnit + " Lost");
		
		
		if (ColorOfUnit.Contains("Blue"))
		{
			GameInfo.RedPlayer.Resources.AmountOfGold += UnitCost * GoldCoefficient;
		}

		if (ColorOfUnit.Contains("Red"))
		{
			GameInfo.BluePlayer.Resources.AmountOfGold += UnitCost * GoldCoefficient;
			
		}


		Debug.Log("Unit is Dead");

		Instantiate(DeathParticleSystem, gameObject.transform.position, Quaternion.Euler(-90,0,0));

		Invoke("PlayDeathSound", 2);
		
		Invoke("DestroyTower", 2.1f);

		GameInfo.GameWinner = GetOpponentColor(ColorOfUnit);
		
		GameInfo.FinishGame();
	}

	/// <summary>
	/// Вызывает звук уничтожения башни
	/// </summary>
	private void PlayDeathSound()
	{
		AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, GameInfo.MusicVolume * GameInfo.VolumeCoefficient * 3);
	}
	
	/// <summary>
	/// Уничтожает башню
	/// </summary>
	private void DestroyTower()
	{
		
		Destroy(gameObject);
	}

	/// <summary>
	/// Выстреливает магическо стрелой в текущего противника
	/// </summary>
	public void ArrowShot()
	{
		if (CurrentTarget == null) return;
		
		PlayRandomAttackSound();
		
		Vector3 startPoint = transform.position;
		startPoint.y += 7;
		GameObject arrow = Instantiate(projectile, startPoint, transform.rotation);
		arrow.SendMessage("SetTarget", CurrentTarget.transform);
		arrow.SendMessage("SetDamage", DamageOfUnit);
	}
	
}
