using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretTargetting
{
    private Transform turretTransform;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool hasTarget => currentTarget != null;
    private bool canShoot => Time.time - timeOfLastShot > secondsBetweenShots;
    private float secondsBetweenShots;
    private float timeOfLastShot;
    public TurretTargetting(Transform turret)
    {
        turretTransform = turret;
    }

    public void HandleAiming(Vector3 rotation)
    {
        if (!hasTarget)
        {
            currentTarget = GetTarget();
        }
        turretTransform.rotation = Quaternion.Euler(rotation);
    }

    private GameObject GetTarget()
    {
        if (targetList != null && targetList.Any())
        {
            return targetList.First();
        }
        return null;
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
    }

    public void OnTriggerExit(Collider other)
    {
        if (currentTarget == other.gameObject) currentTarget = null;
        targetList.Remove(other.gameObject);
    }
    #endregion Events
}
