using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretTargetting
{
    private Transform turretTransform;
    private Quaternion originalOrientation;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool hasTarget => currentTarget != null;
    private bool canShoot => Time.time - timeOfLastShot > secondsBetweenShots;
    private float secondsBetweenShots;
    private float timeOfLastShot;
    private float rotationSpeedRadians = 1;
    private float rotationSpeedDegrees => rotationSpeedDegrees * 180 / Mathf.PI;
    public TurretTargetting(Transform turret)
    {
        turretTransform = turret;
        originalOrientation = turretTransform.rotation;
    }

    public void HandleAiming()
    {
        try
        {
            if (!hasTarget)
            {
                currentTarget = GetTarget();
            }
            if (currentTarget != null)
            {
                turretTransform.LookAt(currentTarget.transform);
                turretTransform.rotation *= Quaternion.Euler(-90, 0, 0);
            }
        }
        catch (System.Exception ex)
        {

            throw;
        }
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
