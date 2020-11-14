﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Health
{
    public event Action FullyHealed;
    [SerializeField]
    private Image healthBar;
    [SerializeField] 
    private float maxHealth = 1;
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            currentHealth = value;
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public bool IsDead => CurrentHealth <= 0;
    public bool FullHealth => CurrentHealth >= maxHealth;
    event EventHandler<GameObject> OnDamaged;
    private GameObject parentObject;
    private bool isInitialized = false;

    public void Initialize(GameObject parent)
    {
        if (!isInitialized)
        {
            parentObject = parent;
            CurrentHealth = maxHealth; 
        }
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0)
            return;
        
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
        OnDamaged?.Invoke(this, parentObject);

        if (IsDead)
            parentObject.GetComponent<IKillable>()?.Kill();
        
    }

    public void Heal()
    {
        CurrentHealth = maxHealth;
    }

    public void Heal(float healAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount, 0, maxHealth);
    }
    
    //public void Regenerate(float hpRegen, float interval) => StartCoroutine(Regen(hpRegen, interval));

    //private IEnumerator Regen(float hpRegen, float interval)
    //{
    //    while (true)
    //    {
    //        Heal(hpRegen);
    //        if (FullHealth)
    //        {
    //            FullyHealed?.Invoke();
    //            break;
    //        }
    //        yield return new WaitForSeconds(interval);
    //    }
    //}

    public void Kill()
    {
        TakeDamage(CurrentHealth);
    } 
}
