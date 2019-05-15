using System.Collections;
using System.Collections.Generic;
using a;
using UnityEngine;

public class CatapultScript : UnitScript
{

	/// <summary>
	/// Бросаемый снаряд
	/// </summary>
	public GameObject projectile;
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		
		
		if (ColorOfUnit == "Blue")
		{
			HealthOfUnit *= ShopUnit.BlueCatapultHealthCoefficient;
			DamageOfUnit *= ShopUnit.BlueCatapultAttackCoefficient;
		}
		
		if (ColorOfUnit == "Red")
		{
			HealthOfUnit *= ShopUnit.RedCatapultHealthCoefficient;
			DamageOfUnit *= ShopUnit.RedCatapultAttackCoefficient;
		}
	}

	
	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	/// <summary>
	/// Бросаеет снаряд в текущую цель
	/// </summary>
	public void ThrowProjectile()
	{
		Debug.Log("\nThrown\n\n\nThrown\n");
		
		
		if (CurrentTarget == null) return;
		
		PlayRandomAttackSound();
		Vector3 startPoint = transform.position;
		startPoint.y += 2;
		
		GameObject tmp = Instantiate(projectile, startPoint, transform.rotation);
		tmp.SendMessage("SetTarget", CurrentTarget.transform);
		tmp.SendMessage("SetDamage", DamageOfUnit);
		tmp.SendMessage("SetColor", GetOpponentColor(ColorOfUnit));
	}

}
