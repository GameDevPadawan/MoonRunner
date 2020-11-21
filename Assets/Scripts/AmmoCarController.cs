using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmmoCarController : CarControllerBase, IReloadable
{
    [SerializeField]
    private int ammoBoxes;
    [SerializeField]
    private int maxAmmoBoxes;
    private bool weHaveAmmo => ammoBoxes > 0;
    private VehicleMovementScript vehicleMovementScript;
    private GameObject cameraObject;
    protected override void Awake()
    {
        base.Awake();
        // We have to give true as a parameter to this to also get componenets from disabled objects
        // Since the car camera is disabed by default this is required
        cameraObject = GetComponentInChildren<Camera>(true).gameObject;
        vehicleMovementScript = GetComponent<VehicleMovementScript>();
        mover.Initialize(this.transform);
        base.interactionType = typeof(IReloadable);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        cameraObject.SetActive(controlEnabled);
        if (controlEnabled)
        {
            vehicleMovementScript.HandleMovement();
            if (InteractPressed())
            {
                TryToInteract();
            }
        }
    }

    bool InteractPressed()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    private void TryToInteract()
    {
        interactableComponents.Sort((x, y) => distanceToInteractable(x.gameObject).CompareTo(distanceToInteractable(y.gameObject)));
        IReloadable nearestInteractable = (IReloadable)interactableComponents.FirstOrDefault();
        if (nearestInteractable != null) HandleReloading(nearestInteractable);

        float distanceToInteractable(GameObject enterableGameObjet)
        {
            return Vector3.Distance(this.gameObject.transform.position, enterableGameObjet.transform.position);
        }
    }

    private void HandleReloading(IReloadable reloadable)
    {
        if (weHaveAmmo)
        {
            reloadable.ReloadFully();
            ammoBoxes--;
        }
    }

    public void ReloadFully()
    {
        this.ammoBoxes = maxAmmoBoxes;
    }

    public void ReloadAmount(int amount)
    {
        this.ammoBoxes += amount;
    }

    public bool NeedsReload()
    {
        return this.ammoBoxes < this.maxAmmoBoxes;
    }
}
