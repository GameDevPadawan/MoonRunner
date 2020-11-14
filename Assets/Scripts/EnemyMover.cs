using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyMover : GenericMover
{
    private Transform[] waypoints;
    int currentWaypointIndex = 0;
    [SerializeField]
    float speed = 10;
    private Vector3 vectorToTarget;
    private Vector3 stopPoint;
    public bool hasRechedTarget => new Vector3(transform.position.x, 0, transform.position.z) == stopPoint;

    public void Initialize(Transform[] waypoints, Transform enemyTransform)
    {
        base.Initialize(enemyTransform);
        this.waypoints = waypoints;
    }

    public void HandlePathMovement()
    {
        if (waypointsIsNotEmpty())
        {
            if (waypoints[currentWaypointIndex] == null)
            {
                return;
            }
            base.MoveTowardsTarget(this.transform.position, waypoints[currentWaypointIndex].transform.position, speed);
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
            if (waypoints == null) return false;
            if (waypoints.Length < 1) return false;
            return true;
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

    public void ApproachTarget(Vector3 targetPos, float distanceToStopFromTarget)
    {
        // To get the vector to the target we subtract where we are from where we need to be to get the path we need to travel.
        vectorToTarget = targetPos - transform.position;
        // We want to ensure we aren't moving vertically
        vectorToTarget.y = 0;
        // To get the step point we have to:
        //  1. create a vector parallel to ours with a magnitude equal to the distance to stop from the target
        //  2. subtract the parallel vector from the path we must travel. 
        //    2.1 This has the effect of shortening the vector to target by our distance to stop from the target.
        //  3. Add this stop position to our current position so we are using the same reference point.
        stopPoint = vectorToTarget - (vectorToTarget.normalized * distanceToStopFromTarget) + transform.position;
        // We want to ensure we aren't moving vertically
        stopPoint.y = 0;
        // Step towards the stop point based on our move speed
        base.MoveTowardsTarget(transform.position, stopPoint, speed);
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, vectorToTarget + transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, stopPoint);
    }
}
