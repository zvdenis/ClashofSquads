using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueNodeScript : MonoBehaviour
{

	/// <summary>
	/// Нажата ли кнопка
	/// </summary>
	public bool IsPressed = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// при нажатии на клетку выделяет ее
	/// </summary>
	private void OnMouseDown()
	{
		BlueBaseScript.SelectedNode = gameObject;
	}


	private void OnMouseUp()
	{
		
	}
}
