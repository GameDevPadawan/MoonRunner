using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TurretTargetting
{
    private Transform turretTransform;
    private Quaternion originalOrientation;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool hasTarget => currentTarget != null;
    private float lastAngleAdjustment;
    [SerializeField]
    private float rotationSpeedRadians = 1;
    private float rotationSpeedDegrees => rotationSpeedDegrees * 180 / Mathf.PI;
    public TurretTargetting(Transform turret)
    {
        turretTransform = turret;
        originalOrientation = turretTransform.rotation;
    }

    public bool IsAimedAtTarget()
    {
        if (currentTarget == null) return false;
        if (lastAngleAdjustment < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HandleAiming()
    {
        if (!hasTarget)
        {
            currentTarget = GetNewTarget();
        }
        if (currentTarget != null)
        {
            Quaternion oldRotation = turretTransform.rotation;
            turretTransform.LookAt(currentTarget.transform);
            turretTransform.rotation *= Quaternion.Euler(-90, 0, 0);
            lastAngleAdjustment = Mathf.Abs(Quaternion.Angle(oldRotation, turretTransform.rotation));
        }
    }

    private GameObject GetNewTarget()
    {
        if (targetList != null && targetList.Any())
        {
            return targetList.First();
        }
        return null;
    }

    public IDamageable GetCurrentTarget()
    {
        return currentTarget == null ? null : currentTarget.GetComponent<IDamageable>();
    }

    #region Events

    public void OnTriggerEnter(Collider other)
    {
        if (!targetList.Contains(other.gameObject))
        {
            targetList.Add(other.gameObject); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (currentTarget == other.gameObject) currentTarget = null;
        targetList.Remove(other.gameObject);
    }
    #endregion Events
}
