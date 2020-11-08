using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMover
{
    private Transform t;

    public GenericMover(Transform gameObjectTransform)
    {
        t = gameObjectTransform;
    }

    public void MoveInDirection(Vector3 direction, float speed, float deltaTime)
    {
        t.position += direction * speed * deltaTime;
    }

    public void MoveTowardsTarget(Vector3 currentPosition, Vector3 targetPosition, float speed, float deltaTime)
    {
        Vector2 currentPos = new Vector2(currentPosition.x, currentPosition.z);
        Vector2 targetPos = new Vector2(targetPosition.x, targetPosition.z);
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * deltaTime);
        Vector3 nextPosition = new Vector3(newPos.x, t.position.y, newPos.y);
        t.position = nextPosition;
    }
}
