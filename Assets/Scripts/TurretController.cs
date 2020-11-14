using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class TurretController : MonoBehaviour, IReloadable, IRepairable
{
    [SerializeField]
    private TurretTargetting targetting;
    [SerializeField]
    private TurretShooting shooting;

    void Awake()
    {
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
            enemy.ReceiveAgro(this.gameObject);
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
            enemy.LoseAgro();
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
    #endregion IReloadable Implementation

    #region IRepairable Implementation
    public void RepairFully()
    {
        throw new System.NotImplementedException();
    }

    public void RepairAmount(float amount)
    {
        throw new System.NotImplementedException();
    }
    #endregion IRepairable Implementation
}
