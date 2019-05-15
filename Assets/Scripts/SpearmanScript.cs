using System.Collections;
using System.Collections.Generic;
using a;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.UIElements;

public class SpearmanScript : UnitScript
{

	/// <summary>
	/// Теущая цель для атаки
	/// </summary>
	protected Collider TargerOfCurrentAttack;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		
		if (ColorOfUnit == "Blue")
		{
			HealthOfUnit *= ShopUnit.BlueSpearmanHealthCoefficient;
			DamageOfUnit *= ShopUnit.BlueSpearmanAttackCoefficient;
		}
		
		if (ColorOfUnit == "Red")
		{
			HealthOfUnit *= ShopUnit.RedSpearmanHealthCoefficient;
			DamageOfUnit *= ShopUnit.RedSpearmanAttackCoefficient;
		}
	}
	
	// Update is called once per frame
	protected  override void Update () {
		base.Update();
		if(CurrentTarget != null)
		UnitAgent.destination = CurrentTarget.transform.position;
		else
		{
			IsMoving = IsAttacking = false;
		}
		if (ListOfEnemies.Count <= 0)
		{
			UnitAgent.destination = GameObject.FindGameObjectWithTag("Current" + ColorOfUnit + "Target").transform.position;
		}
		if (UnitAgent.remainingDistance - UnitAgent.radius <= AttackRadius)
		{
			UnitAgent.isStopped = true;
			IsAttacking = true;
			TargerOfCurrentAttack = CurrentTarget;
		}
		else
		{
			UnitAgent.isStopped = false;
		}
	}
	
	/// <summary>
	/// Добавляет врага в список для атаки при столкновении
	/// </summary>
	/// <param name="other"></param>
	protected override void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.tag.Contains(GetOpponentColor(ColorOfUnit) + "Body")) return;
		ListOfEnemies.Add(other);
	}

	/// <summary>
	/// Удаляет врага из списка
	/// </summary>
	/// <param name="other"></param>
	protected override void OnTriggerExit(Collider other)
	{
		if (!other.tag.Contains(GetOpponentColor(ColorOfUnit))) return;
		ListOfEnemies.Remove(other);
		if (ListOfEnemies.Count <= 0)
		{
		}
	}

	/// <summary>
	/// Наносит урон текущей цели
	/// </summary>
	public void DealDamage()
	{
		if (TargerOfCurrentAttack != null && UnitAgent.isActiveAndEnabled)
		{
			if (UnitAgent.remainingDistance - UnitAgent.radius <= AttackRadius)
			{
				PlayRandomAttackSound();
				TargerOfCurrentAttack.transform.parent.SendMessage("GetDamage", -DamageOfUnit);
			}
		}
	}
	
}
