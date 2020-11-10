using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Health
{
    public event Action FullyHealed;
    
    [SerializeField] private float maxHealth = 1; // TODO remove this hard coded value once we can set this in the inspector properly.
    public float CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;
    public bool FullHealth => CurrentHealth >= maxHealth;
    event EventHandler<GameObject> OnDamaged;
    private GameObject parentObject;
    public Health(GameObject parent)
    {
        parentObject = parent;
        CurrentHealth = maxHealth;
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
