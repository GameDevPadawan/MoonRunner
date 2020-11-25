using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaypointMoveable
{
    void SignalWaypointReached(WaypointNode waypointNodeReched);
}
