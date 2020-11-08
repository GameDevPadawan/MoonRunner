using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover
{
    public GameObject[] waypoints;

    int currentWaypointIndex = 0;
    float speed;
    private Transform transform;
    private GenericMover mover;

    public EnemyMover(GameObject[] waypoints, Transform enemyTransform, float moveSpeed)
    {
        this.waypoints = waypoints;
        transform = enemyTransform;
        speed = moveSpeed;
        mover = new GenericMover(transform);
    }

    public void HandleMovement()
    {
        if (waypointsIsNotEmpty())
        {
            mover.MoveTowardsTarget(this.transform.position, waypoints[currentWaypointIndex].transform.position, speed, Time.deltaTime);
            if (hasReachedWaypoint())
            {
                if (!atLastWaypoint())
                {
                    currentWaypointIndex++;
                }
            }
        }

        bool waypointsIsNotEmpty()
        {
            return waypoints.Length > 0;
        }

        bool hasReachedWaypoint()
        {
            Vector2 targetPos = new Vector2(waypoints[currentWaypointIndex].transform.position.x, waypoints[currentWaypointIndex].transform.position.z);
            Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.z);
            float distance = Vector2.Distance(currentPos, targetPos);
            return Mathf.Approximately(distance, 0);
        }

        bool atLastWaypoint()
        {
            return currentWaypointIndex == waypoints.Length - 1;
        }
    }
}
