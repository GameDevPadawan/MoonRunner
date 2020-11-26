using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable, IKillable, IWaypointMoveable
{
    [SerializeField]
    private Vector3[] Waypoints;
    [SerializeField]
    private NavMeshMover mover;
    private GameObject agroTarget;
    private bool hasAgro => agroTarget != null;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private float secondsBetweenShots = 1;
    private float timeOfLastShot;
    private bool canShoot => Time.time - timeOfLastShot > secondsBetweenShots;
    [SerializeField]
    private Health health;
    public Health Health => health;
    public event EventHandler<GameObject> OnDeath;
    [SerializeField]
    private Enemy enemyScriptableObject;
    public Enemy ScriptableObject => enemyScriptableObject;
    [SerializeField]
    private GameObject[] scrapsWeCanSpawn;

    private void OnDrawGizmosSelected()
    {
        mover.DrawGizmos();
    }

    void Awake()
    {
        health.Initialize(this.gameObject, enemyScriptableObject.maxHealth);
        mover.Initialize(GetComponent<NavMeshAgent>(), Waypoints);
    }

    void Update()
    {
        HandleMovement();
        if (agroTarget != null)
        {
            IDisableable disableableTarget = agroTarget.GetComponent<IDisableable>();
            if (disableableTarget != null)
            {
                if (disableableTarget.IsDisabled())
                {
                    agroTarget = null;
                }
            }
        }
        if (agroTarget != null)
        {
            IDamageable damageableTarget = agroTarget.GetComponent<IDamageable>();
            if (damageableTarget != null)
            {
                // TODO (Nate) fix enemy so they move to the target and then shoot.
                //      This was tough because the nav agents make it hard to know when they are done moving.
                //      For now enemies will shoot as long as they have a target.
                if (canShoot /*&& mover.HasReachedTarget*/) shoot(damageableTarget);
            }
        }
    }

    // TODO this should probably be its' own class.
    void shoot(IDamageable damageableTarget)
    {
        timeOfLastShot = Time.time;
        damageableTarget.TakeDamage(this.damage);
    }

    void HandleMovement()
    {
        if (mover == null) return;
        if (hasAgro)
        {
            mover.HandleTargetMovement(agroTarget.transform.position, 10);
        }
        else if (mover != null)
        {
            mover.HandlePathMovement();
        }
    }

    public void Kill()
    {
        // UnityEngine.Random selects a random scrap to spawn. We could replace this with a scrap pool and let it handle the random selection.
        // We use Range from 0 to array.Length because this method uses the upper bound as exclusive instead of inclusive like the lower bound.
        Instantiate(scrapsWeCanSpawn[UnityEngine.Random.Range(0, scrapsWeCanSpawn.Length)], this.transform.position, this.transform.rotation);
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

    public void SetWaypoints(Vector3[] waypoints)
    {
        Waypoints = waypoints;
        mover.SetWaypoints(Waypoints);
    }

    public void SignalWaypointReached(WaypointNode waypointNodeReched)
    {
        ((IWaypointMoveable)mover).SignalWaypointReached(waypointNodeReched);
    }
}
