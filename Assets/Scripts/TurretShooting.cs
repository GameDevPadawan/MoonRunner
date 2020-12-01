using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TurretShooting
{
    [SerializeField]
    private string shootSoundEffectName;
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
    [SerializeField]
    private bool hasInfiniteAmmo = false;
    public bool HasInfiniteAmmo => hasInfiniteAmmo;
    private AudioManager audioManager;
    public event Action Reloaded;

    public void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            RefillAmmo();
            audioManager = GameObject.FindObjectOfType<AudioManager>();
        }
    }

    public void HandleShooting(IDamageable target)
    {
        Initialize();
        if (canShoot)
        {
            shoot(target);
        }
    }

    private void shoot(IDamageable target)
    {
        if (!hasInfiniteAmmo) AmmoCount--;
        target.TakeDamage(damagePerShot);
        timeOfLastShot = Time.time;
        audioManager.PlayOneShot(shootSoundEffectName);
    }

    public void RefillAmmo()
    {
        AmmoCount = maxAmmo;
        Reloaded?.Invoke();
    }

    public void RefillAmmo(int amount)
    {
        AmmoCount = amount;
        Reloaded?.Invoke();
    }

    public bool NeedsReload()
    {
        return this.AmmoCount < this.maxAmmo;
    }
}
