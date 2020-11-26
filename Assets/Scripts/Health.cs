using System;
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
    public float MaxHealth => maxHealth;
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
            if (!IsDead)
            {
                parentObject.GetComponent<IDisableable>()?.Enable();
            }
        }
    }

    public bool IsDead => CurrentHealth <= 0;
    public bool FullHealth => CurrentHealth >= maxHealth;
    event EventHandler<GameObject> OnDamaged;
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
        OnDamaged?.Invoke(this, parentObject);

        if (IsDead)
        {
            parentObject.GetComponent<IKillable>()?.Kill();
            parentObject.GetComponent<IDisableable>()?.Disable();
        }
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
