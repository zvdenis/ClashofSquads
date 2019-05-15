using System.Collections;
using System.Collections.Generic;
using a;
using UnityEngine;
using UnityEngine.AI;

public class ArcherScript : UnitScript
{
	
	/// <summary>
	/// Объект выпускаемый лучником
	/// </summary>
	public GameObject projectile;
	
	protected override void Start ()
	{
		base.Start();
		if (ColorOfUnit == "Blue")
		{
			HealthOfUnit *= ShopUnit.BlueArcherHealthCoefficient;
			DamageOfUnit *= ShopUnit.BlueArcherAttackCoefficient;
		}
		
		if (ColorOfUnit == "Red")
		{
			HealthOfUnit *= ShopUnit.RedArcherHealthCoefficient;
			DamageOfUnit *= ShopUnit.RedArcherAttackCoefficient;
		}
	}
	
	protected override void Update () 
	{
		base.Update();
	}

	protected override void  AttackEnemy()
	{
		ArrowShot();
	}
	
	
	/// <summary>
	/// Выпускает стрелу в противника
	/// </summary>
	public void ArrowShot()
	{
		if (CurrentTarget == null) return;
		
		PlayRandomAttackSound();
		Debug.Log("Archer shot");
		Vector3 startPoint = transform.position;
		startPoint.y += 1;
		GameObject arrow = Instantiate(projectile, startPoint, transform.rotation);
		arrow.SendMessage("SetTarget", CurrentTarget.transform);
		arrow.SendMessage("SetDamage", DamageOfUnit);
	}
	
}
