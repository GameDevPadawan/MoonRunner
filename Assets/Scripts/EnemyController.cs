using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable, IKillable, IWaypointMoveable
{
    [SerializeField]
    private Vector3[] Waypoints;
    [SerializeField]
    private NavMeshMover mover;
    private GameObject agroTarget;
    private float distanctToAgroTarget => hasAgro ? Vector3.Distance(transform.position, agroTarget.transform.position) : float.PositiveInfinity;
    private bool hasAgro => agroTarget != null;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private float secondsBetweenShots = 1;
    private float timeOfLastShot;
    private bool shootIntervalElapsed => Time.time - timeOfLastShot > secondsBetweenShots;
    private bool closeEnoughToShoot => hasAgro && distanctToAgroTarget < targettingSphereCollider.radius;
    private bool canShoot => shootIntervalElapsed && closeEnoughToShoot;
    [SerializeField]
    private Health health;
    public Health Health => health;
    public event EventHandler<GameObject> OnDeath;
    [SerializeField]
    private Enemy enemyScriptableObject;
    public Enemy ScriptableObject => enemyScriptableObject;
    [SerializeField]
    private GameObject[] scrapsWeCanSpawn;
    private List<GameObject> nearbyIdamageables = new List<GameObject>();
    private SphereCollider targettingSphereCollider;

    private void OnDrawGizmosSelected()
    {
        mover.DrawGizmos();
    }

    void Awake()
    {
        targettingSphereCollider = GetComponent<SphereCollider>();
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
                    LoseAgro(agroTarget);
                }
            }
        }
        if (agroTarget != null)
        {
            IDamageable damageableTarget = agroTarget.GetComponent<IDamageable>();
            if (damageableTarget != null)
            {
                if (canShoot) shoot(damageableTarget);
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
        // UnityEngine.Random helps us select a random scrap to spawn. 
        //   We could replace this with a scrap pool and let it handle the random selection.
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

    private void ReceiveAgro(GameObject gameObject)
    {
        nearbyIdamageables.Add(gameObject);
        if (agroTarget == null)
        {
            agroTarget = nearbyIdamageables.FirstOrDefault();
        }
    }

    private void LoseAgro(GameObject gameObject)
    {
        nearbyIdamageables.Remove(gameObject);
        if (agroTarget == gameObject)
        {
            agroTarget = nearbyIdamageables.FirstOrDefault();
        }
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public bool IsValidTarget()
    {
        return true;
    }

    public TargetTypes GetTargetType()
    {
        return TargetTypes.Enemy;
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

    private void OnTriggerEnter(Collider other)
    {
        IDamageable otherAsDamageable = other.gameObject.GetComponent<IDamageable>();
        if (otherAsDamageable != null 
            && otherAsDamageable.IsValidTarget() 
            && otherAsDamageable.GetTargetType() == TargetTypes.Friendly)
        {
            ReceiveAgro(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable otherAsDamageable = other.gameObject.GetComponent<IDamageable>();
        if (otherAsDamageable != null
            && otherAsDamageable.IsValidTarget()
            && otherAsDamageable.GetTargetType() == TargetTypes.Friendly)
        {
            LoseAgro(other.gameObject);
        }
    }
}
