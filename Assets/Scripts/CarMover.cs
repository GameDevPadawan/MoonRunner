using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarMover : GenericMover
{
    [SerializeField]
    private float speed = 20;

    public void Initialize(Transform carTransform)
    {
		// We do not start sitting in a car so they should not move until we enter one.
		base.Enabled = false;
		base.Initialize(carTransform);
	}

	public void HandleMovement()
	{
		Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		// normalize the vector so we do no move faster when moving diagonally
		// Simple explanation here: http://answers.unity.com/answers/1291321/view.html
		moveDirection.Normalize();
		base.MoveInDirection(moveDirection, speed);
	}
}
