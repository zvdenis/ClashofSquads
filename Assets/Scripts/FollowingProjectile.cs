using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowingProjectile : MonoBehaviour {

	/// <summary>
	/// Расстояние на котором происходит удар стрелы
	/// </summary>
	public float Eps = 0.001f;
	/// <summary>
	/// Цель за которой следует снаряд
	/// </summary>
	private Transform target;
	/// <summary>
	/// Урон снаряда
	/// </summary>
	private float damage;
	/// <summary>
	/// Скорость снаряда
	/// </summary>
	public float speed;
	/// <summary>
	/// Эффект при нанесении урона
	/// </summary>
	public GameObject particles;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target);
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
		transform.Rotate(-90f,0,0);

		if (target == null)Destroy(gameObject);
		try
		{
			if (Vector3.Distance(transform.position, target.position) < Eps)
			{
				Hit();
			}
		}
		catch (MissingReferenceException)
		{
			Debug.Log("Ссылка null уничтожен обьект");
			Destroy(gameObject);	
		}
	}

	
	/// <summary>
	/// Наносит урон цели и проигрывает эффект
	/// </summary>
	private void Hit()
	{
		GameObject tmp = Instantiate(particles, transform.position,transform.rotation);
		Destroy(tmp,4);
		
		target.parent.SendMessage("GetDamage", -damage);
		Destroy(gameObject);
		
	}
	
	/// <summary>
	/// Устанавливает цель снаряда
	/// </summary>
	/// <param name="v"></param>
	public void SetTarget(Transform v)
	{
		target = v;
	}

	/// <summary>
	/// Устанавливает урон снаряда
	/// </summary>
	/// <param name="v"></param>
	public void SetDamage(float v)
	{
		damage = v;
	}
}
