using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
	public float speed = 6.0F;
	private GenericMover mover;

	void Awake()
	{
		mover = new GenericMover(this.transform);
	}

	void Update()
	{
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		// normalize the vector so we do no move faster when moving diagonally
		// Simple explanation here: http://answers.unity.com/answers/1291321/view.html
		moveDirection.Normalize();
		mover.MoveInDirection(moveDirection, speed, Time.deltaTime);
	}
}
