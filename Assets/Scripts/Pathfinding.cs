using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour {

	/// <summary>
	/// Список которые должен посетить юнит
	/// </summary>
	public Transform[] points;

	/// <summary>
	/// Агент навигации
	/// </summary>
	private NavMeshAgent nav;
	/// <summary>
	/// текущая цель движения
	/// </summary>
	private int destPoint;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!nav.pathPending && nav.remainingDistance < 0.5f) {
			GoToNextPoint ();
		}
	}
	
	/// <summary>
	/// Отправляется к следующей точке
	/// </summary>
	void GoToNextPoint()
	{
		if (points.Length == 0)
			return;
		nav.destination = points [destPoint].position;
		destPoint = (destPoint + 1) % points.Length;
	}
}
