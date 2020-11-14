using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TurretTargetting
{
    [SerializeField]
    private Transform turretTransform;
    [SerializeField]
    private Transform barrelTransform;
    private Quaternion originalOrientation;
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool hasTarget => currentTarget != null;
    private float lastAngleAdjustment;
    // start debug code for weird turret rotations
    [SerializeField]
    private bool useCustomRotations;
    [SerializeField]
    private float xRotation;
    [SerializeField]
    private float yRotation;
    [SerializeField]
    private float zRotation;
    // end debug code for weird turret rotations
    [SerializeField]
    private float rotationSpeedRadians = 1;
    private float rotationSpeedDegrees => rotationSpeedRadians * 180 / Mathf.PI;
    private bool isInitialized = false;

    public void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            originalOrientation = turretTransform.rotation; 
        }
    }

    public bool IsAimedAtTarget()
    {
        if (currentTarget == null) return false;
        // hacky way to tell if we are done aiming.
        // This will be changed once the aiming and orintation is working.
        // It should calculate the ange between our forward and the target orientation.
        Vector3 directionToTarget = (currentTarget.transform.position - barrelTransform.position).normalized;
        float angle = Quaternion.Angle(barrelTransform.rotation, Quaternion.LookRotation(directionToTarget));
        if (angle < 1)
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
        Initialize();
        if (!hasTarget)
        {
            currentTarget = GetNewTarget();
        }
        if (currentTarget != null)
        {
            //RotateWholeTurretImmediately();
            //InefficientAiming();
            StepRotationTowardsCurrentTarget();
        }
    }

    private void StepRotationTowardsCurrentTarget()
    {
        Quaternion currentTurretRotation = turretTransform.rotation;
        Quaternion currentBarrelRotation = barrelTransform.rotation;
        // Just let unity do it's thing and aim us at the target.
        turretTransform.LookAt(currentTarget.transform);
        barrelTransform.LookAt(currentTarget.transform);
        Quaternion desiredTurretRotation = turretTransform.rotation;
        Quaternion desiredBarrelRotation = barrelTransform.rotation;
        turretTransform.rotation = currentTurretRotation;
        barrelTransform.rotation = currentBarrelRotation;
        Quaternion steppedTurretRotation = Quaternion.RotateTowards(currentTurretRotation, desiredTurretRotation, rotationSpeedDegrees * Time.deltaTime);
        Quaternion steppedBarrelRotation = Quaternion.RotateTowards(currentBarrelRotation, desiredBarrelRotation, rotationSpeedDegrees * Time.deltaTime);
        turretTransform.rotation = Quaternion.Euler(0, steppedTurretRotation.eulerAngles.y, 0);
        barrelTransform.rotation = Quaternion.Euler(steppedBarrelRotation.eulerAngles.x, barrelTransform.rotation.eulerAngles.y, barrelTransform.rotation.eulerAngles.z);
        Debug.Log($"Desired:{desiredTurretRotation.eulerAngles} | Stepped:{steppedTurretRotation.eulerAngles}");
    }

    private void RotateWholeTurretImmediately()
    {
        // end debug code for weird turret rotations
        if (useCustomRotations)
        {
            turretTransform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
        // end debug code for weird turret rotations
        else
        {
            // store old rotation so we can get the amount changed for the hacky lastAngleAdjustment variable
            Quaternion oldRotation = turretTransform.rotation;
            // Just let unity do it's thing and aim us at the target.
            turretTransform.LookAt(currentTarget.transform);
            // We have to rotate around the y axis
            // This would normally make sense, but watch this objects transform. The rotation shows as a z axis rotation...
            turretTransform.rotation = Quaternion.Euler(0, turretTransform.rotation.eulerAngles.y, 0);
            //turretTransform.rotation *= Quaternion.Euler(-90, 0, 0);
            // update this for the hacky way of checking to see if we are done aiming
            lastAngleAdjustment = Mathf.Abs(Quaternion.Angle(oldRotation, turretTransform.rotation));
        }
    }

    private void InefficientAiming()
    {
        // end debug code for weird turret rotations
        if (useCustomRotations)
        {
            turretTransform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }
        // end debug code for weird turret rotations
        else
        {
            // store old rotation so we can get the amount changed for the hacky lastAngleAdjustment variable
            Quaternion oldRotation = turretTransform.rotation;
            Quaternion desiredRotation = Quaternion.LookRotation(currentTarget.transform.position - turretTransform.position);
            float angleFromTurretToTarget = desiredRotation.eulerAngles.y - (turretTransform.rotation.eulerAngles.y - originalOrientation.eulerAngles.y);
            if (Mathf.Abs(angleFromTurretToTarget) > 1)
            {
                turretTransform.Rotate(0, 0, Mathf.Sign(angleFromTurretToTarget) * rotationSpeedDegrees * Time.deltaTime);
            }
            lastAngleAdjustment = Mathf.Abs(Quaternion.Angle(oldRotation, turretTransform.rotation));
            float angleFromBarrelToTarget = desiredRotation.eulerAngles.x - (barrelTransform.rotation.eulerAngles.x - originalOrientation.eulerAngles.x);
            if (Mathf.Abs(angleFromBarrelToTarget) > 1)
            {
                barrelTransform.Rotate(Mathf.Sign(angleFromBarrelToTarget) * rotationSpeedDegrees * Time.deltaTime, 0, 0);
            }
            Debug.Log($"Turret:{angleFromTurretToTarget} | Barrel:{angleFromBarrelToTarget}");
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
