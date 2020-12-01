using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Health
{
    public event Action OnHealed;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private float maxHealth = 1;
    public float MaxHealth => maxHealth;
    [SerializeField]
    private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value;
            if (healthBar) healthBar.fillAmount = currentHealth / maxHealth;
            if (!IsDead)
            {
                parentObject.GetComponent<IDisableable>()?.Enable();
            }
        }
    }

    public bool IsDead => CurrentHealth <= 0;
    public bool IsFullHealth => CurrentHealth >= maxHealth;
    public event Action OnDamaged;
    private GameObject parentObject;
    private bool isInitialized = false;

    public void Initialize(GameObject parent, float maximumHealth = -1)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            parentObject = parent;
            if (maximumHealth != -1)
            {
                this.maxHealth = maximumHealth;
            }
            CurrentHealth = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isInitialized) throw new Exception($"Health must be initialized before trying to take damage.");

        if (IsDead || damage <= 0)
            return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
        OnDamaged?.Invoke();

        if (IsDead)
        {
            parentObject.GetComponent<IKillable>()?.Kill();
            parentObject.GetComponent<IDisableable>()?.Disable();
        }
    }

    public void Heal()
    {
        CurrentHealth = maxHealth;
        OnHealed?.Invoke();
    }

    public void Heal(float healAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount, 0, maxHealth);
        OnHealed?.Invoke();
    }

    public void Kill()
    {
        TakeDamage(CurrentHealth);
    }
}
