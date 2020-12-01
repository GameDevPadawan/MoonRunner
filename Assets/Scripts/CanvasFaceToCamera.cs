using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaceToCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (mainCamera == null)
        {
            // This is a patch on a REAL PROBLEM

            // TODO change how this script works. If this is registered to a car camera it will forever point to that camera
            //   This will cause health bars to point at whatever camera they were initialized with.
            //   We either need only one camera that moves around or we need ways to update the active camera when it is changed
            //   This could be a very costly check every frame if we do it wrong. Need to be careful.
            mainCamera = Camera.main;
        }*/
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
