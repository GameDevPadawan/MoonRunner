using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TurretShooting
{
    [SerializeField]
    private float damagePerShot;
    [SerializeField]
    private float secondsBetweenShots;
    private bool canShoot => shotDelayElapsed && hasAmmo;
    private bool hasAmmo => ammoCount > 0;
    private bool shotDelayElapsed => Time.time - timeOfLastShot > secondsBetweenShots;
    private float timeOfLastShot;
    [SerializeField]
    private int ammoCount;
    [SerializeField]
    private int maxAmmo;
    private bool isInitialized = false;

    public void Initialize()
    {
        if (!isInitialized)
        {
            ammoCount = maxAmmo; 
        }
    }

    public void HandleShooting(IDamageable target)
    {
        Initialize();
        if (canShoot)
        {
            ammoCount--;
            target.TakeDamage(damagePerShot);
            timeOfLastShot = Time.time;
        }
    }

    public void RefillAmmo()
    {
        ammoCount = maxAmmo;
    }

    public void RefillAmmo(int amount)
    {
        ammoCount = amount;
    }
}
