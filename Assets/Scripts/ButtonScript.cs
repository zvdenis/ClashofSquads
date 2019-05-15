using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

	/// <summary>
	/// Модель размещаемого юнита
	/// </summary>
	public GameObject dull;

	
	//Что бы сэкономить время просто перемещаем одного юнита вместо создания новых
	
	/// <summary>
	/// Перемещаемый на клетки юнит
	/// </summary>
	private GameObject existingDull;
	
	
	private void Start()
	{
		existingDull = Instantiate(dull, Input.mousePosition, Quaternion.identity);
		existingDull.tag = "Untagged";
	}




	
	/// <summary>
	/// Перемещает модель юнита по карте 
	/// </summary>
	/// <param name="eventData"></param>
	public void OnDrag(PointerEventData eventData)
	{
		//transform.position = Input.mousePosition;
		
		existingDull.GetComponent<Collider>().enabled = true;
		CameraMoveScript.IsUnitDragged = true;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			Vector3 placeTarget = hit.point;
			placeTarget.y += 0.4f;
			existingDull.transform.position = placeTarget;

//			if (hit.transform.tag.Contains("BlueNode"))
//			{
//				BlueBaseScript.SelectedNode = hit.transform.gameObject;
//			}
		}

		
		
		//existingDull.transform.position = Input.mousePosition;
	}


	private void Update()
	{
		
	}

	/// <summary>
	/// При завершении перемещения проверяет верно ли поставлен юнит
	/// </summary>
	/// <param name="eventData"></param>
	public void OnEndDrag(PointerEventData eventData)
	{
		existingDull.GetComponent<Collider>().enabled = false;
		CameraMoveScript.IsUnitDragged = false;

		
		int unitSize = existingDull.GetComponent<DullScript>().SizeOfUnit;

		if (unitSize == DullScript.CollidedNodes.Count)
		{
			
			BlueBaseScript.PlaceOnList(dull, DullScript.CollidedNodes);
		}
		
//		foreach (var node in DullScript.CollidedNodes)
//		{
//		}

		
		
		
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//		if (Physics.Raycast(ray, out hit)&& hit.transform.tag.Contains("BlueNode"))
//		{
//			BlueBaseScript.PlaceOnTargetNode(dull);
//		}
		
		
		existingDull.transform.Translate(new Vector3(0,-20f,0));
		
		DullScript.CollidedNodes.Clear();
	}


	/// <summary>
	/// Если кнопка нажата ил перетаскивается записывает это в переменную
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerEnter(PointerEventData eventData)
	{
		CameraMoveScript.IsUnitDragged = true;
	}

	/// <summary>
	/// Если кнопку перестали нажимать записывает это в переменную
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerExit(PointerEventData eventData)
	{
		CameraMoveScript.IsUnitDragged = false;
	}
}
