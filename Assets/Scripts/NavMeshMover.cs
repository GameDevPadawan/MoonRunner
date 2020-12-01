using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[Serializable]
public class NavMeshMover : IWaypointMoveable
{
    public bool isInitialized = false;
    private NavMeshAgent agent;
    private Queue<Vector3> moveQueue;
    private bool waypointInQueue => moveQueue != null && moveQueue.Any();



    #region Agro Target Logic
    /// <summary>
    /// <para>This <b>is not</b> this position of the target.</para>
    /// <para>This <b>is</b> the vector between us and the target AT THE MOMENT THE TARGET WAS SET.</para>
    /// </summary>
    private Vector3 vectorToTarget;
    /// <summary>
    /// Where we should stop moving. This will stop us close to the target but offset by some radius.
    /// </summary>
    private Vector3 stopPoint;

    public bool HasReachedTarget => agent.transform.position == stopPoint;
    #endregion Agro Target Logic

    public void Initialize(NavMeshAgent navMeshAgent, Vector3[] waypoints)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            agent = navMeshAgent;
            UpdateMoveQueue(waypoints); 
        }
    }

    public void HandlePathMovement()
    {
        if (waypointInQueue && isInitialized)
        {
            agent.SetDestination(moveQueue.Peek());
        }
    }

    public void HandleTargetMovement(Vector3 targetPosition, float distanceToStopFromTarget)
    {
        // To get the vector to the target we subtract where we are from where we need to be to get the path we need to travel.
        vectorToTarget = targetPosition - agent.transform.position;
        // We want to ensure we aren't moving vertically
        //vectorToTarget.y = 0;
        // To get the step point we have to:
        //  1. create a vector parallel to ours with a magnitude equal to the distance to stop from the target
        //  2. subtract the parallel vector from the path we must travel. 
        //    2.1 This has the effect of shortening the vector to target by our distance to stop from the target.
        //  3. Add this stop position to our current position so we are using the same reference point.
        stopPoint = vectorToTarget - (vectorToTarget.normalized * distanceToStopFromTarget) + agent.transform.position;
        // We want to ensure we aren't moving vertically
        //stopPoint.y = 0;
        // Step towards the stop point based on our move speed
        agent.SetDestination(stopPoint);
    }

    public void SetWaypoints(Vector3[] waypoints)
    {
        UpdateMoveQueue(waypoints);
    }

    private void UpdateMoveQueue(Vector3[] waypoints)
    {
        moveQueue = new Queue<Vector3>(waypoints);
    }

    void IWaypointMoveable.SignalWaypointReached(WaypointNode waypointNodeReched)
    {
        if (waypointNodeReched != null && waypointNodeReched.gameObject != null && moveQueue.Contains(waypointNodeReched.transform.position))
        {
            moveQueue.Dequeue();
        }
    }

    #region Gizmos
    public void DrawGizmos()
    {
        if (agent.transform != null && stopPoint != null && vectorToTarget != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(agent.transform.position, vectorToTarget + agent.transform.position);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(agent.transform.position, stopPoint);
        }
    }
    #endregion Gizmos
}
