using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

public class CameraMoveScript : MonoBehaviour
{

	/// <summary>
	/// Перемещается ли юнит на поле
	/// </summary>
	public static bool IsUnitDragged = false;
	/// <summary>
	/// Скорость камеры
	/// </summary>
	public float Speed;

	
	//Для удобства границы были представлены в виде 4 игровых объектов которые позволяют увидеть границы
	
	/// <summary>
	/// Верхнаяя граница камеры
	/// </summary>
	public Transform TopBorder;
	
	/// <summary>
	/// Нижняя граница камеры
	/// </summary>
	public Transform DownBorder;
	
	/// <summary>
	/// Левая граница камеры
	/// </summary>
	public Transform LeftBorder;
	
	/// <summary>
	/// Правая граница камеры
	/// </summary>
	public Transform RightBorder;

	/// <summary>
	/// События перемещения к левой границе
	/// </summary>
	public static EventDelegate MoveToLeftBorder;
	/// <summary>
	/// События перемещения к правой границе
	/// </summary>
	public static EventDelegate MoveToRightBorder;
	
	// Use this for initialization
	void Start () {
		MoveToLeftBorder = delegate
		{
			Vector3 tmp = Camera.main.transform.position;
			tmp.z = LeftBorder.transform.position.z;
			Camera.main.transform.position = tmp;
		};
		MoveToRightBorder = delegate
		{
			Vector3 tmp = Camera.main.transform.position;
			tmp.z = RightBorder.transform.position.z;
			Camera.main.transform.position = tmp;
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Переместить камеру к левой границе
	/// </summary>
	public void MoveLeft()
	{
		CameraMoveScript.MoveToLeftBorder();
	}
	
	/// <summary>
	/// Переместить камеру к правой границе 
	/// </summary>
	public void MoveRight()
	{
		CameraMoveScript.MoveToRightBorder();
	}
	void OnMouseDrag()
	{
		if(! IsUnitDragged)
		{
			
			
//			if (EventSystem.current.IsPointerOverGameObject())
//			{
//				return;
//			}
			float X = Input.GetAxis("Mouse X");
			float Y = Input.GetAxis("Mouse Y");

			//X и Y - движение игрока по осям на экране
			Camera.main.transform.Translate(Y * Speed, 0, -X * Speed, Space.World);
			Vector3 tmp = Camera.main.transform.position;
				
			
			
			if (tmp.x < TopBorder.position.x)
			{
				tmp.x = TopBorder.position.x;
			}
			if (tmp.x > DownBorder.position.x)
			{
				tmp.x = DownBorder.position.x;
			}
			if (tmp.z < LeftBorder.position.z)
			{
				tmp.z = LeftBorder.position.z;
			}
			if (tmp.z > RightBorder.position.z)
			{
				tmp.z = RightBorder.position.z;
			}

			Camera.main.transform.position = tmp;
		}

		
		  


	}
}
