using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catapult_script : MonoBehaviour
{

	public float gravity = 9.8f;

	public float speed = 10f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float deltaX = Input.GetAxis("Horizontal");
		float deltaZ = Input.GetAxis("Vertical") * speed;
		transform.Translate(0, 0, deltaZ * Time.deltaTime);
		transform.Rotate(0,deltaX,0);
	}
}
