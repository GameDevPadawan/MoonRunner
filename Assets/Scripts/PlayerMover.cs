using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
	public float speed = 6.0F;

	private Vector2 moveDirection = Vector2.zero;

	void Start()
	{
	}

	void Update()
	{
		// Use input up and down for direction, multiplied by speed
		moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed * Time.deltaTime;
		transform.position += (Vector3)moveDirection;
	}
}
