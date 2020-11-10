using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TurretShooting
{
    [SerializeField]
    private float damagePerShot = 1; // TODO remove this hard coded value once we can set this in the inspector properly.
    [SerializeField]
    private float secondsBetweenShots = 1; // TODO remove this hard coded value once we can set this in the inspector properly.
    private bool canShoot => shotDelayElapsed && hasAmmo;
    private bool hasAmmo => ammoCount > 0;
    private bool shotDelayElapsed => Time.time - timeOfLastShot > secondsBetweenShots;
    private float timeOfLastShot;
    [SerializeField]
    private int ammoCount;
    [SerializeField]
    private int maxAmmo = 4; // TODO remove this hard coded value once we can set this in the inspector properly.

    public TurretShooting()
    {
        ammoCount = maxAmmo;
    }

    public void HandleShooting(IDamageable target)
    {
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
}
