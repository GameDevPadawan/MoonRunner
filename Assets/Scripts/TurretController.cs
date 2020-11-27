using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class TurretController : MonoBehaviour, IReloadable, IRepairable, IDamageable, IDisableable
{
    [SerializeField]
    private TurretTargetting targetting;
    [SerializeField]
    private TurretShooting shooting;
    [SerializeField]
    private Health health;
    private bool disabled;
    private bool Disabled
    {
        get
        {
            return disabled;
        }
        set
        {
            disabled = value;
            this.enabled = !disabled;
        }
    }

    void Awake()
    {
        health.Initialize(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        targetting.HandleAiming();
        if (targetting.IsAimedAtTarget())
        {
            shooting.HandleShooting(targetting.GetCurrentTarget());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (targetting != null)
            {
                targetting.OnTriggerEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            if (targetting != null)
            {
                targetting.OnTriggerExit(other);
            }
        }
    }

    #region IReloadable Implementation
    public void ReloadFully()
    {
        shooting.RefillAmmo();
    }

    public void ReloadAmount(int amount)
    {
        shooting.RefillAmmo(amount);
    }

    public bool NeedsReload()
    {
        return shooting.NeedsReload();
    }
    #endregion IReloadable Implementation

    #region IRepairable Implementation
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
    #endregion IRepairable Implementation

    #region IDamageable Implementation
    public void TakeDamage(float amount)
    {
        health.TakeDamage(amount);
    }

    public bool IsValidTarget()
    {
        // Turrets are valid targets unless they have infinite ammo.
        //   This is needed because the base turrets should not be targetted.
        return !this.shooting.HasInfiniteAmmo;
    }

    public TargetTypes GetTargetType()
    {
        return TargetTypes.Friendly;
    }
    #endregion IDamageable Implementation

    #region IDisableable Implementation
    public void Disable()
    {
        Disabled = true;
    }

    public void Enable()
    {
        Disabled = false;
    }

    public bool IsDisabled()
    {
        return Disabled;
    }
    #endregion IDisableable Implementation
}
