using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NewVehicleController : MonoBehaviour
{
    [Header("Control Options")]
    [SerializeField]
    private bool fourWheelDrive;
    [SerializeField]
    private float maxSteerAngle;
    [SerializeField]
    private float maxTorque, torqueChangeRate, maxSpeed;
    
    [Header("Read-outs [Do not edit]")]
    [SerializeField]
    private float currentTorque;
    [SerializeField]
    private float currentSpeed;
    [Header("FL,FR,BL,BR = x,y,z,w")]
    [SerializeField]
    private Vector4 forwardSlip;
    [SerializeField]
    private Vector4 sidewaysSlip;
    [SerializeField]
    private Vector4 forces;
    

    [Header("Center of Mass Control - Do not change without good reason")]
    [SerializeField]
    private Vector3 centerOfMassOffset;
    [SerializeField]
    private Vector3 localCenterOfMass;
    private Vector3 defaultCenterOfMass;

    // Leave these at the bottom of the serialized fields so they are out of the way.
    [Header("Wheel Meshes")]
    [SerializeField]
    private GameObject frontLeftMesh;
    [SerializeField]
    private GameObject frontRightMesh;
    [SerializeField]
    private GameObject backLeftMesh;
    [SerializeField]
    private GameObject backRightMesh;
    [Header("Wheel Colliders")]
    [SerializeField]
    private WheelCollider frontLeftCollider;
    [SerializeField]
    private WheelCollider frontRightCollider;
    [SerializeField]
    private WheelCollider backLeftCollider;
    [SerializeField]
    private WheelCollider backRightCollider;

    [Header("Sliding")]
    [SerializeField]
    private float minSlideStiffness;
    [SerializeField]
    private float defaultStiffness;
    [SerializeField]
    private float enterSlideStiffnessChangeRate;
    [SerializeField]
    private float exitSlideStiffnessChangeRate;
    [SerializeField]
    private float currentSlideStiffness;

    private Vector2 input;
    private Quaternion defaultRotation;
    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultRotation = Quaternion.Euler(rb.rotation.eulerAngles.x, 0, rb.rotation.eulerAngles.z);
        defaultCenterOfMass = rb.centerOfMass;
        rb.centerOfMass = defaultCenterOfMass + centerOfMassOffset;
        localCenterOfMass = rb.centerOfMass;
        defaultStiffness = backLeftCollider.sidewaysFriction.stiffness;
    }

    public void SetInput(Vector2 input) => this.input = input;

    void FixedUpdate()
    {
        HandleSteering(input.x);
        HandleAcceleration(input.y);
        UpdateWheelMeshes();

        UpdateReadOuts();
        
    }

    private void UpdateReadOuts()
    {
        var fl = GetWheelHitData(frontLeftCollider);
        var fr = GetWheelHitData(frontRightCollider);
        var bl = GetWheelHitData(backLeftCollider);
        var br = GetWheelHitData(backRightCollider);

        forwardSlip = new Vector4(fl.fSlip, fr.fSlip, bl.fSlip, br.fSlip);
        sidewaysSlip = new Vector4(fl.sSlip, fr.sSlip, bl.sSlip, br.sSlip);
        forces = new Vector4(fl.force, fr.force, bl.force, br.force);

        (float fSlip, float sSlip, float force) GetWheelHitData(WheelCollider wheel)
        {
            if (wheel.GetGroundHit(out WheelHit hit))
            {
                return (hit.forwardSlip, hit.sidewaysSlip, hit.force);
            }
            else
            {
                return (0, 0, 0);
            }
        }
    }

    private void UpdateWheelMeshes()
    {
        UpdateWheelMesh(frontLeftMesh, frontLeftCollider);
        UpdateWheelMesh(frontRightMesh, frontRightCollider);
        UpdateWheelMesh(backLeftMesh, backLeftCollider);
        UpdateWheelMesh(backRightMesh, backRightCollider);

        void UpdateWheelMesh(GameObject wheelMesh, WheelCollider wheelCollider)
        {
            wheelCollider.GetWorldPose(out Vector3 FRposition, out Quaternion FRquat);
            wheelMesh.transform.position = FRposition;
            wheelMesh.transform.rotation = FRquat;
        }
    }

    private void HandleSteering(float inputXAxis)
    {
        float steerAngle = inputXAxis * maxSteerAngle;
        frontLeftCollider.steerAngle = steerAngle;
        frontRightCollider.steerAngle = steerAngle;
    }

    private void HandleAcceleration(float inputYAxis)
    {
        float desiredTorque = inputYAxis * maxTorque;
        currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < maxSpeed)
        {
            currentTorque = Mathf.Lerp(currentTorque, desiredTorque, torqueChangeRate * Time.deltaTime); 
        }
        else
        {
            currentTorque = 0;
        }
        frontLeftCollider.motorTorque = currentTorque;
        frontRightCollider.motorTorque = currentTorque;
        if (fourWheelDrive)
        {
            backRightCollider.motorTorque = currentTorque;
            backLeftCollider.motorTorque = currentTorque;
        }
    }


    public void Slide(bool slidePressed)
    {
        float changeRate = slidePressed ? enterSlideStiffnessChangeRate : exitSlideStiffnessChangeRate;
        float desiredSiffness = slidePressed ? minSlideStiffness : defaultStiffness;
        currentSlideStiffness = Mathf.Lerp(currentSlideStiffness, desiredSiffness, changeRate * Time.deltaTime);
        // debug testing
        //currentSlideStiffness = desiredSiffness;
        //
        var backLeftFriction = backLeftCollider.sidewaysFriction;
        var backRightFriction = backRightCollider.sidewaysFriction;
        backLeftFriction.stiffness = currentSlideStiffness;
        backRightFriction.stiffness = currentSlideStiffness;
        backLeftCollider.sidewaysFriction = backLeftFriction;
        backRightCollider.sidewaysFriction = backRightFriction;
    }


    public void SetRotationToUpright()
    {
        rb.transform.rotation = Quaternion.Euler(defaultRotation.eulerAngles.x, rb.rotation.eulerAngles.y, defaultRotation.eulerAngles.z);
    }
}
