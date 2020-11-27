using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MainBaseController : MonoBehaviour, IDamageable, IKillable, IRepairable
{
    public event EventHandler<GameObject> OnDeath;
    [SerializeField]
    private Health health;
    public Health Health => health;

    private void Awake()
    {
        health.Initialize(this.gameObject);
    }

    void IKillable.Kill()
    {
        Debug.Log("You lose. Have you tried sucking less?");
    }

    void IDamageable.TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    bool IDamageable.IsValidTarget()
    {
        return true;
    }

    public TargetTypes GetTargetType()
    {
        return TargetTypes.Friendly;
    }

    public void RepairFully()
    {
        health.Heal();
    }

    public void RepairAmount(float amount)
    {
        health.Heal(amount);
    }

    public float GetRepairAmountNeeded()
    {
        return health.MaxHealth - health.CurrentHealth;
    }
}
