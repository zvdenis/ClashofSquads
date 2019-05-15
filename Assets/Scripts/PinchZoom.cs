using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
	/// <summary>
	/// Скорость приближения камеры
	/// </summary>
	public float perspectiveZoomSpeed = 0.001f; 
	

	/// <summary>
	/// Камера которую необходимо приблизить
	/// </summary>
	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
	}
	
	void Update()
	{
		
		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


			mainCamera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

			mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, 30f, 90f);

		}
	}
}