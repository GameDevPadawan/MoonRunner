using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarMover : GenericMover
{
    [SerializeField]
    private float speed = 20;

    public CarMover(Transform t) : base(t)
    {
		// We do not start sitting in a car so they should not move until we enter one.
		base.Enabled = false;
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
