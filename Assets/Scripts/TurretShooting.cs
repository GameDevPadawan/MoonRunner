using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int AmmoCount
    {
        get
        {
            return ammoCount;
        }
        private set
        {
            ammoCount = value;
            ammoBar.fillAmount = (float)ammoCount / (float)maxAmmo;
        }
    }
    [SerializeField]
    private Image ammoBar;
    [SerializeField]
    private int maxAmmo;
    private bool isInitialized = false;

    public void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            RefillAmmo();
        }
    }

    public void HandleShooting(IDamageable target)
    {
        Initialize();
        if (canShoot)
        {
            AmmoCount--;
            target.TakeDamage(damagePerShot);
            timeOfLastShot = Time.time;
        }
    }

    public void RefillAmmo()
    {
        AmmoCount = maxAmmo;
    }

    public void RefillAmmo(int amount)
    {
        AmmoCount = amount;
    }
}
