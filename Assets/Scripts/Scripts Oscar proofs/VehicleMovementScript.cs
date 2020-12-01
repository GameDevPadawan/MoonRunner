using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VehicleMovementScript
{
    public float Speed;
    public float MaxSteeringAngle;
    public bool FRrot, FLrot, BRrot, BLrot;
    public Quaternion defaultRotation;
    public float mH, mV;


    [Header("Center of Mass Control - Do not change without good reason")]
    [SerializeField]
    private Vector3 centerOfMassOffset;
    [SerializeField]
    private Vector3 localCenterOfMass;
    private Vector3 defaultCenterOfMass;

    //Complex drive with wheels collider
    [Header("Put here de object with wheel collider")]
    [SerializeField]
    WheelCollider FrontRight, BackRight, FrontLeft, BackLeft;
    private float sidewaysFrictionDefaultValue;

    // This is for the child wheel meshes.
    [Header("Put here the childs with wheel meshes")]
    [SerializeField]
    GameObject FRM, BRM, FLM, BLM;
    [SerializeField]
    bool FourXFour;

    [Header("Don´t change the values below, only for check")]
    [SerializeField]
    float accel;
    [SerializeField]
    float steer;
    [SerializeField]
    bool PlayerClose;
    Rigidbody rb;

    private bool isInitialized;

    public void Initialize(Rigidbody rigidbody)
    {
        if (!isInitialized)
        {
            isInitialized = true;
            rb = rigidbody;
            defaultRotation = Quaternion.Euler(rb.rotation.eulerAngles.x, 0, rb.rotation.eulerAngles.z);
            defaultCenterOfMass = rb.centerOfMass;
            sidewaysFrictionDefaultValue = FrontLeft.sidewaysFriction.extremumValue;
        }
    }

 
    public void HandleMovement(Vector2 input)
    {
        if (!isInitialized) throw new Exception($"{this.GetType().Name} must be intialized by the MonoBehavior using it.");
        rb.centerOfMass = defaultCenterOfMass + centerOfMassOffset;
        localCenterOfMass = rb.centerOfMass;
        accel = input.y;
        steer = input.x;
        Go();
    }

    public void SetRotationToUpright()
    {
        rb.transform.rotation = Quaternion.Euler(defaultRotation.eulerAngles.x, rb.rotation.eulerAngles.y, defaultRotation.eulerAngles.z);
    }

    void Go()
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * MaxSteeringAngle;
        float thrustTorqueLeft = accel * Speed;
        float thrustTorqueRight = accel * -Speed;
        if (FourXFour == true)
        {
            FrontRight.motorTorque = thrustTorqueLeft;
            FrontLeft.motorTorque = thrustTorqueLeft;
            BackRight.motorTorque = thrustTorqueLeft;
            BackLeft.motorTorque = thrustTorqueLeft;
        }

        //Spin meshes
        // Be sure that the wesh has the rotation aplied.
        Quaternion FRquat;
         Vector3 FRposition;
         FrontRight.GetWorldPose(out FRposition, out FRquat);
         FRM.transform.position = FRposition;
         FRM.transform.rotation = FRquat;
        if (FRrot == true)
        {
            FrontRight.steerAngle = steer;
        }

        Quaternion FLquat;
        Vector3 FLposition;
        FrontLeft.GetWorldPose(out FLposition, out FLquat);
        FLM.transform.position = FLposition;
        FLM.transform.rotation = FLquat;

        if (FLrot == true)
        {
            FrontLeft.steerAngle = steer;
        }

        Quaternion BRquat;
        Vector3 BRposition;
       BackRight.GetWorldPose(out BRposition, out BRquat);
        BRM.transform.position = BRposition;
        BRM.transform.rotation = BRquat;

        if (BRrot == true)
        {
            BackRight.steerAngle = -steer;
        }

        Quaternion BLquat;
        Vector3 BLposition;
       BackLeft.GetWorldPose(out BLposition, out BLquat);
        BLM.transform.position = BLposition;
        BLM.transform.rotation = BLquat;

        if (BLrot == true)
        {
            BackLeft.steerAngle = -steer;
        }


    }
}
