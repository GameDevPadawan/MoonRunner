using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VehicleMovementScript : MonoBehaviour
{
    public bool ActiveCar;
    public Camera CarCam;
    public bool SimpleDriving;
    public float Speed;
    public float MaxSteeringAngle;
    public bool FRrot, FLrot, BRrot, BLrot;
    float DefaultRotX, DefaultRotZ;
    public float mH, mV;

    //Complex drive with wheels collider
    [Header("Put here de object with wheel collider")]
    [SerializeField]
    WheelCollider FrontRight, BackRight, FrontLeft, BackLeft;

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

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        CarCam = GetComponentInChildren<Camera>();
        CarCam.gameObject.SetActive(false);
       
        DefaultRotX = transform.rotation.x;
        DefaultRotZ = transform.rotation.z;

    }

 
    public void HandleMovement()
    {
        mH = Input.GetAxis("Horizontal");
        mV = Input.GetAxis("Vertical");

        if (SimpleDriving == false)
        {
            accel = Input.GetAxis("Vertical");
            steer = Input.GetAxis("Horizontal");
            Go();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnOverturn();
        }
    }

    void UnOverturn()
    {
        transform.rotation = new Quaternion(DefaultRotX, transform.rotation.y, DefaultRotZ,transform.rotation.w);
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

    public void EnterExitCar()
    {
        if(ActiveCar== false)
        {
            CarCam.gameObject.SetActive(true);
            ActiveCar = true;
        }
        else if (ActiveCar == true)
        {
            CarCam.gameObject.SetActive(false);
            ActiveCar = false;
        }
    }
}
