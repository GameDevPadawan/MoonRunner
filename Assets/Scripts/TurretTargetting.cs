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
    private List<GameObject> targetList = new List<GameObject>();
    private GameObject currentTarget;
    private bool hasTarget => currentTarget != null;
    [SerializeField]
    private float rotationSpeedRadians = 1;
    private float rotationSpeedDegrees => rotationSpeedRadians * 180 / Mathf.PI;
    private bool isInitialized = false;
    private AudioManager audioManager;

    /// <summary>
    /// Initialize is meant to replace any actions that would ususally happen in the constructor.
    /// </summary>
    public void Initialize()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            audioManager = GameObject.FindObjectOfType<AudioManager>();
        }
    }

    /// <summary>
    /// Calulates the angle between the barrel and the target.
    /// </summary>
    /// <returns>
    /// <para><b>True</b> if the angle between target and barrel is small.</para>
    /// <para><b>False</b> if the angle between target and barrel is large.</para>
    /// </returns>
    public bool IsAimedAtTarget()
    {
        if (!hasTarget) return false;

        Vector3 directionToTarget = (currentTarget.transform.position - barrelTransform.position).normalized;
        float angleToTarget = Quaternion.Angle(barrelTransform.rotation, Quaternion.LookRotation(directionToTarget));
        float maxAngleToShootAtTarget = 1;

        if (angleToTarget < maxAngleToShootAtTarget)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Rotates the turret and barrel towards the <see cref="currentTarget"/> at a rate defined by <see cref="rotationSpeedRadians"/>.
    /// </summary>
    public void HandleAiming()
    {
        Initialize();
        if (!hasTarget)
        {
            currentTarget = GetNewTarget();
        }
        if (hasTarget && !IsAimedAtTarget())
        {
            StepRotationTowardsCurrentTarget();
        }
    }

    private void StepRotationTowardsCurrentTarget()
    {
        audioManager.PlayOneShot("turretRotation");
        // Store our starting rotation so we can use it to step ttowards the desired rotation
        Quaternion currentTurretRotation = turretTransform.rotation;
        Quaternion currentBarrelRotation = barrelTransform.rotation;
        // We want to calculate this rotation from the barrel so we only have to do it once.
        // This gives us the up and down for the barrel AND the left and right for the turret.
        barrelTransform.LookAt(currentTarget.transform);
        // LookAt just snaps the rotation to the target so we want to store where we should be aiming
        Quaternion desiredRotation = barrelTransform.rotation;
        // reset the rotations so we don't actually snap to the target
        // (These rotations don't get applied until we exit so this seems safe)
        turretTransform.rotation = currentTurretRotation;
        barrelTransform.rotation = currentBarrelRotation;
        Quaternion steppedTurretRotation = Quaternion.RotateTowards(currentTurretRotation, desiredRotation, rotationSpeedDegrees * Time.deltaTime);
        Quaternion steppedBarrelRotation = Quaternion.RotateTowards(currentBarrelRotation, desiredRotation, rotationSpeedDegrees * Time.deltaTime);
        // We only ever want the turret to rotate about the y-axis(vertical axis)
        turretTransform.rotation = Quaternion.Euler(0, steppedTurretRotation.eulerAngles.y, 0);
        // We let the barrel rotate about all axes with the turret, except the x-axis which lets us point up and down to the target.
        barrelTransform.rotation = Quaternion.Euler(steppedBarrelRotation.eulerAngles.x, barrelTransform.rotation.eulerAngles.y, barrelTransform.rotation.eulerAngles.z);
    }

    private GameObject GetNewTarget()
    {
        if (targetList != null && targetList.Any())
        {
            GameObject newTarget = targetList.First();
            if (newTarget == null)
            {
                targetList.RemoveAt(0);
                return GetNewTarget();
            }
            return newTarget;
        }
        return null;
    }

    /// <summary>
    /// Gets the target the turret is actively tracking. This may return null if there is no current target.
    /// </summary>
    /// <returns>Returns the <see cref="IDamageable"/> interface for the current target or null if there is no current target.</returns>
    public IDamageable GetCurrentTarget()
    {
        return currentTarget == null ? null : currentTarget.GetComponent<IDamageable>();
    }

    #region Events

    public void AddToTargetList(GameObject target)
    {
        if (!targetList.Contains(target))
        {
            targetList.Add(target); 
        }
    }

    public void RemoveFromTargetList(GameObject target)
    {
        if (currentTarget == target) currentTarget = null;
        targetList.Remove(target);
    }
    #endregion Events
}
