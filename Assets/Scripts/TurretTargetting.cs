using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretTargetting
{
    private Transform turretTransform;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool canShoot => Time.time - timeOfLastShot > secondsBetweenShots;
    private float secondsBetweenShots;
    private float timeOfLastShot;
    public TurretTargetting(Transform turret)
    {
        turretTransform = turret;
    }

    public void HandleAiming()
    {

    }


    public void HandleShooting()
    {
        if (canShoot)
        {
            // TODO shoot
            timeOfLastShot = Time.time;
        }
    }

    #region Events

    public void OnTriggerEnter(Collider other)
    {
        targetList.Add(other.gameObject);
        if (currentTarget == null) currentTarget = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        if (currentTarget == other.gameObject) currentTarget = null;
        targetList.Remove(other.gameObject);
        if (targetList.Count > 0)
        {
            currentTarget = targetList.First();
        }
    }
    #endregion Events
}
