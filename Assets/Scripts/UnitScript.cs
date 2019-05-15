using System;
using System.Collections;
using System.Collections.Generic;
using a;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public delegate void EventDelegate();

//Базовый класс для всех юнитов
public class UnitScript : MonoBehaviour
{
	/// <summary>
	/// Урон юнита
	/// </summary>
	public float DamageOfUnit = 1;
	/// <summary>
	/// Скорость юнита
	/// </summary>
	public int SpeedOfUnit = 1;
	/// <summary>
	/// Список противников в досягаемости атаки
	/// </summary>
	protected List<Collider> ListOfEnemies;
	/// <summary>
	/// Радиус обнаружения противника
	/// </summary>
	public float AttackDetectRadius = 1;

	/// <summary>
	/// Радиус аттаки
	/// </summary>
	public float AttackRadius; 

	/// <summary>
	/// Источник звука юнита
	/// </summary>
	private AudioSource Audio;

	/// <summary>
	/// Список звуков с которыми атакует юнит
	/// </summary>
	public AudioClip[] AttackSounds;
	
	/// <summary>
	/// Событие изменения здоровья
	/// </summary>
	public event EventDelegate OnHpChanged;
	
	/// <summary>
	/// Коэффицент золота за убийство противников
	/// </summary>
	public static float GoldCoefficient = 0.2f;
	
	/// <summary>
	/// Аниматор юнито
	/// </summary>
	protected Animator UnitAnimator;
	/// <summary>
	/// Навигационный агент юнита
	/// </summary>
	protected NavMeshAgent UnitAgent;

	/// <summary>
	/// Стоимость юнита
	/// </summary>
	public float UnitCost;
	
	/// <summary>
	/// Дистанция прекращения анимации движения
	/// </summary>
	protected double EpsOfMoving = 0.5d;

	/// <summary>
	/// Движется ли юнит
	/// </summary>
	private bool isMoving = false;

	/// <summary>
	/// Движется ли юнит
	/// </summary>
	protected bool IsMoving
	{
		
		get
		{
			return isMoving;
			
		}
		set
		{
			ChangeAnimation();
			isMoving = value;
			
		}
	}

	private bool isAttacking = false;
	/// <summary>
	/// Атакует ли юнит
	/// </summary>
	protected bool IsAttacking
	{
		get
		{
			return isAttacking;
		}
		set
		{
			ChangeAnimation();
			isAttacking = value;
			
		}
	}

	/// <summary>
	/// Цвет юнита
	/// </summary>
	protected string ColorOfUnit = "Blue";
	/// <summary>
	///  Максимальное здоровье юнита
	/// </summary>
	public float HealthOfUnit = 10;
	/// <summary>
	/// Текущее здоровье юнита
	/// </summary>
	protected  float currentHealth = 10;

	/// <summary>
	/// Здоровье юнита
	/// </summary>
	public float CurrentHealth
	{
		get { return currentHealth;}
		set
		{
			currentHealth = value;
			if (currentHealth <= 0)
			{
				currentHealth = 0;
				UnitDied();
			}

			if (currentHealth > HealthOfUnit)
			{
				currentHealth = HealthOfUnit;
			}
			
			if(OnHpChanged != null)
				OnHpChanged.Invoke();
		}
	}

	/// <summary>
	/// Текущая цель для атаки
	/// </summary>
	public Collider CurrentTarget
	{
		get
		{
			if (ListOfEnemies.Count > 0)
				return FindClosestAgent();

			return null;
		}
	}

	/// <summary>
	/// Проигрывает случайны звук из списка звуков
	/// </summary>
	protected void PlayRandomAttackSound()
	{
		Audio.volume = GameInfo.MusicVolume * GameInfo.VolumeCoefficient;
		Audio.clip = AttackSounds[Random.Range(0, AttackSounds.Length)];
		Audio.Play();
	}
	
	/// <summary>
	/// Выбирает ближайшего юнита для атаки
	/// </summary>
	/// <returns></returns>
	protected Collider FindClosestAgent()
	{
		float range = 111111f;
		Collider tmp = null;
		DeleteNulls();
		for (int i = 0; i < ListOfEnemies.Count; i++)
		{
			if(ListOfEnemies[i] != null)
			if (range > Vector3.Distance(transform.position, ListOfEnemies[i].transform.position))
			{
				
				range = Vector3.Distance(transform.position, ListOfEnemies[i].transform.position);
				tmp = ListOfEnemies[i];
			}
		}

		return tmp;
	}

	/// <summary>
	/// Удаляте далеких или умерших юнитов из списка атаки
	/// </summary>
	protected void DeleteNulls()
	{
		for (int i = 0; i < ListOfEnemies.Count;i++)
		{
			if (ListOfEnemies[i] == null)
			{
				ListOfEnemies.RemoveRange(i, 1);
			}
		}
	}
	
	
	
	
	// Use this for initialization
	protected virtual void Start () {
		UnitAnimator = GetComponent<Animator>();
		UnitAgent = GetComponent<NavMeshAgent>();
		
		

		currentHealth = HealthOfUnit;
		ColorOfUnit = GetColorFromTag();
		UnitAgent.speed = SpeedOfUnit;
		ListOfEnemies = new List<Collider>();
		GetComponent<SphereCollider>().radius = AttackDetectRadius;
		Audio = gameObject.GetComponent<AudioSource>();
		
	}

	/// <summary>
	/// Возвращает цвет юнита, определяя его по тегу
	/// </summary>
	/// <returns></returns>
	private string GetColorFromTag()
	{
		if (tag.Contains("Red")) return "Red";
		if (tag.Contains("Blue")) return "Blue";
		return "NoColor";
	}

	
	// Update is called once per frame
	protected virtual void Update () 
	{
		CheckForRunning();
		ChangeAnimation();
		
		if (ListOfEnemies.Count <= 0)
		{
			IsAttacking = false;
			
			if(UnitAgent.isOnNavMesh)
			UnitAgent.isStopped = false;
			
			return;
		}


		if (CurrentTarget == null) return;
		Vector3 direction = (CurrentTarget.transform.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    
		
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
	}

	/// <summary>
	/// Проверяет необходима ли анимация бега
	/// </summary>
	protected virtual void CheckForRunning()
	{
		if (UnitAgent.velocity.magnitude < EpsOfMoving)
		{
			IsMoving = false;
		}
		else
		{
			IsMoving = true;
		}
	}

	/// <summary>
	/// Обновляет анимации
	/// </summary>
	protected virtual void ChangeAnimation()
	{
		UnitAnimator.SetBool("move", IsMoving);
		UnitAnimator.SetBool("attack", IsAttacking);
	}

	/// <summary>
	/// При обнаружении столкновения проверяем на тело противника
	/// </summary>
	/// <param name="other"></param>
	protected virtual void OnTriggerEnter(Collider other)
	{
		if (!other.tag.Contains(GetOpponentColor(ColorOfUnit) + "Body")) return;
		ListOfEnemies.Add(other);
		IsAttacking = true;
		if(UnitAgent.isOnNavMesh)
		UnitAgent.isStopped = true;
	}
	/// <summary>
	/// Уничтожает экземпляр юнита
	/// </summary>
	protected virtual void UnitDied()
	{
		
		
		//Дает противнику золото за убийство юнита
//		if (ColorOfUnit.Contains("Blue"))
//		{
//			GameInfo.RedPlayer.Resources.AmountOfGold += UnitCost * GoldCoefficient;
//		}
//
//		if (ColorOfUnit.Contains("Red"))
//		{
//			GameInfo.BluePlayer.Resources.AmountOfGold += UnitCost * GoldCoefficient;
//			
//		}


		Debug.Log("Unit is Dead");
		Destroy(gameObject);
	}
	
	/// <summary>
	/// При выходе объекта удалем его из списка
	/// </summary>
	/// <param name="other"></param>
	protected virtual void OnTriggerExit(Collider other)
	{
		if (!other.tag.Contains(GetOpponentColor(ColorOfUnit))) return;
		ListOfEnemies.Remove(other);
		if (ListOfEnemies.Count <= 0)
		{
			IsAttacking = false;
			UnitAgent.isStopped = false;
		}
	}

	/// <summary>
	/// Возвращает цвет оппонента по цвету юнита
	/// </summary>
	/// <param name="clr">Цвет юнита</param>
	/// <returns>Цвет противника</returns>
	public static string GetOpponentColor(string clr)
	{
		switch (clr)
		{
			case "Blue":
				return "Red";
			case "Red":
				return "Blue";
			default: throw new ArgumentException("Wrong Color in Opponent color");
		}
	}

	/// <summary>
	/// Атакует противника
	/// </summary>
	protected virtual void AttackEnemy()
	{
	}
	
	/// <summary>
	/// Наносит юниту урон
	/// </summary>
	/// <param name="v">количество урона</param>
	public void GetDamage(float v)
	{
		CurrentHealth += v;
		
	}
}
