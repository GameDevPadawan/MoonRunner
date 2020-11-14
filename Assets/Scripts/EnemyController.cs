using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IKillable
{
    [SerializeField]
    private Transform[] Waypoints;
    [SerializeField]
    private EnemyMover mover;
    private GameObject agroTarget;
    private bool hasAgro => agroTarget != null;
    [SerializeField]
    private Health health;
    public event EventHandler<GameObject> OnDeath;

    void Awake()
    {
        health.Initialize(this.gameObject);
        mover.Initialize(Waypoints, this.transform, 10);
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (mover == null) return;
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
        health.Kill();
        OnDeath?.Invoke(this, this.gameObject);
        // TODO is this a race condition?
        // Destroying a game object makes the unity engine return true if we run a gameObject == null check
        // We use the enemy death in the wave spawner to remove the enemy from the list of spawned(and still alive) enemies
        // If some subscriber to the OnDeath event cheked for null we might intruduce a bug.
        // Can we delay destruction long enough for the callbacks to happen?
        // Or, could we make it the subscirbers problem, meaning subscribers to OnDeath know not to check for null equality?
        // (Late addition comment: I added a delay so we can see the add force. This delay should be removed after the add force.)
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

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public void SetWaypoints(Transform[] waypoints)
    {
        Waypoints = waypoints;
        mover.Initialize(Waypoints, this.transform, 10);
    }
}
