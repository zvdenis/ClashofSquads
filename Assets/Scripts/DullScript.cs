using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DullScript : MonoBehaviour
{

	/// <summary>
	/// Юнит который будет поставлен на базе синего игрока
	/// </summary>
	public GameObject gObject;
	
	/// <summary>
	/// Юнит который будет поставлен на базе красного игрока
	/// </summary>
	public GameObject gObjectRed;
	/// <summary>
	/// количество клеток занимыаемых юниотм
	/// </summary>
	public int SizeOfUnit = 1;


	/// <summary>
	/// Список выделенных клеток
	/// </summary>
	public static List<Collider> CollidedNodes = new List<Collider>();
	
	/// <summary>
	/// Расстояние на котором появляются новые юниты
	/// </summary>
	private float SpawnDistance = 0; 
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Размещает нового юнита 
	/// </summary>
	public void Spawn()
	{
		Vector3 target = gameObject.transform.position;

		if (gameObject.tag.Contains(("Blue")))
		{
			target.z += SpawnDistance;
			GameObject tmp = Instantiate(gObject,target,new Quaternion(0,0,0,0));
			tmp.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("CurrentBlueTarget").transform.position;
			tmp.tag = "UnitBlue";
		}
		else
		{
			target.z -= SpawnDistance;
			
			GameObject tmp = Instantiate(gObjectRed,target,new Quaternion(0,0,-1,0));
			tmp.GetComponent<NavMeshAgent>().destination = GameObject.FindGameObjectWithTag("CurrentRedTarget").transform.position;
			tmp.tag = "UnitRed";
		}

	}
	
	/// <summary>
	/// Выделяет клетку и добваляет в список
	/// </summary>
	/// <param name="other"></param>
	protected  void OnTriggerEnter(Collider other)
	{
		if (other.tag.Contains("Node"))
		{
			CollidedNodes.Add(other);
			other.GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
		}
	}

	
	/// <summary>
	/// Отменяет выделениу и удаляет из списка клетку
	/// </summary>
	/// <param name="other"></param>
	protected  void OnTriggerExit(Collider other)
	{
		if (other.tag.Contains("Node"))
		{
			CollidedNodes.Remove(other);
			if(other.tag.Contains("Blue"))
				other.GetComponent<Renderer>().material.color = BlueBaseScript.StandartBlueColor;
			else
				other.GetComponent<Renderer>().material.color = BlueBaseScript.StandartRedColor;
		}
	}
}
