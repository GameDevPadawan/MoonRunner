using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        UIManager.Message("Your main base died. You lose...");
        StartCoroutine(Delay(3));
    }

    IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UIManager.UnlockAndRevealCursor();
        SceneManager.LoadScene("MainMenu");
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
