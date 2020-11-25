using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class WaypointNode : MonoBehaviour
{
    void Start()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
        collider.height = 100;
        collider.radius = 2;
        //collider.center = this.transform.position;
        // direction can be 0,1,2 (X,Y,Z) respectively.
        collider.direction = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        IWaypointMoveable otherAsWaypointMoveable = other.GetComponent<IWaypointMoveable>();
        if (otherAsWaypointMoveable != null)
        {
            otherAsWaypointMoveable.SignalWaypointReached(this);
        }
    }
}
