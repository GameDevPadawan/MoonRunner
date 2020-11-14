using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenericMover
{
    /// <summary>
    /// We can use this to turn off a mover during certain situations. 
    /// For example, when the player is in a car we should disable the player mover.
    /// </summary>
    public bool Enabled = true;
    protected Transform transform;

    protected void Initialize(Transform gameObjectTransform)
    {
        transform = gameObjectTransform;
    }

    protected void MoveInDirection(Vector3 direction, float speed, float deltaTime)
    {
        if (!Enabled) return;
        Vector3 oldPos = transform.position;
        transform.position += direction * speed * deltaTime;
        rotateToFaceMoveDirection(transform.position - oldPos);
    }

    protected void MoveTowardsTarget(Vector3 currentPosition, Vector3 targetPosition, float speed, float deltaTime)
    {
        if (!Enabled) return;
        Vector3 oldPos = transform.position;
        Vector2 currentPos = new Vector2(currentPosition.x, currentPosition.z);
        Vector2 targetPos = new Vector2(targetPosition.x, targetPosition.z);
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * deltaTime);
        Vector3 nextPosition = new Vector3(newPos.x, transform.position.y, newPos.y);
        transform.position = nextPosition;
        rotateToFaceMoveDirection(transform.position - oldPos);
    }

    private void rotateToFaceMoveDirection(Vector3 movementDirection)
    {
        // If we do not check this the inspector will get a lot of logs saying "Look Rotation Viewing Vector Is Zero"
        // They are not exceptions, but it's probably best to minimize and logs that are happening.
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection); 
        }
    }
}
