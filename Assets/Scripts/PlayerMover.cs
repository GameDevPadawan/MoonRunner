using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerMover : GenericMover
{
	public float speed = 6.0F;

	public void Initialize(Transform playerTransform)
	{
		base.Initialize(playerTransform);
	}

	public void HandleMovement()
	{
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		// normalize the vector so we do no move faster when moving diagonally
		// Simple explanation here: http://answers.unity.com/answers/1291321/view.html
		moveDirection.Normalize();
		base.MoveInDirection(moveDirection, speed, Time.deltaTime);
	}
}
