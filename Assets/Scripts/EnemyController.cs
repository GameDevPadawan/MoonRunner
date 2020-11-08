using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] Waypoints;
    private EnemyMover mover;
    // Start is called before the first frame update
    void Awake()
    {
        mover = new EnemyMover(Waypoints, this.transform, 10);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (mover != null)
        {
            mover.HandleMovement();
        }
    }
}
