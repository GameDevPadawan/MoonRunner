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
        t.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * deltaTime);
    }
}
