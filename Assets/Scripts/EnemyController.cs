using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] Waypoints;
    private EnemyMover mover;
    private GameObject agroTarget;
    private bool hasAgro => agroTarget != null;

    public event EventHandler<GameObject> OnDeath;

    void Awake()
    {
        mover = new EnemyMover(Waypoints, this.transform, 10);
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (hasAgro)
        {
            mover.ApproachTarget(agroTarget.transform.position, 10);
        }
        else if (mover != null)
        {
            mover.HandlePathMovement();
        }
    }

    public void Kill()
    {
        OnDeath?.Invoke(this, this.gameObject);
        Destroy(this.gameObject);
    }

    public void ReceiveAgro(GameObject gameObject)
    {
        agroTarget = gameObject;
    }

    public void LoseAgro()
    {
        agroTarget = null;
    }
}
