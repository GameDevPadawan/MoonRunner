using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewPlayerController : MonoBehaviour, IReloadable, IDamageable, IKillable, IRepairable
{
    [SerializeField]
    private int ammoBoxes;
    [SerializeField]
    private int maxAmmoBoxes;
    private bool weHaveAmmo => ammoBoxes > 0;
    [SerializeField]
    private VehicleMovementScript vehicleMovementScript;
    [SerializeField]
    private Health health;
    public Health Health => health;
    private List<Type> supportedInteractionInterfaces = new List<Type>();
    [SerializeField]
    private List<GameObject> nearbyInteractables = new List<GameObject>();
    public event EventHandler<GameObject> OnDeath;
    [SerializeField]
    private Scrap scrap;

    protected void Awake()
    {
        supportedInteractionInterfaces = new List<Type>() { typeof(IReloadable), typeof(IRepairable) };
        vehicleMovementScript.Initialize(GetComponent<Rigidbody>());
        health.Initialize(this.gameObject);
    }

    protected void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        vehicleMovementScript.HandleMovement(input);
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        if (spacePressed) vehicleMovementScript.SetRotationToUpright();
        if (InteractPressed())
        {
            TryToInteract();
        }
    }

    bool InteractPressed()
    {
        return Input.GetKeyDown(KeyCode.R);
    }

    private void TryToInteract()
    {
        nearbyInteractables.Sort((x, y) => distanceToInteractable(x.gameObject).CompareTo(distanceToInteractable(y.gameObject)));

        // TODO We need to use separate buttons for reload and repair.
        //   OR we need to do only one or the other when the button is pressed.

        IReloadable reloadable = nearbyInteractables.FirstOrDefault().GetComponent<IReloadable>();
        if (reloadable != null) HandleReloading(reloadable);

        IRepairable repairable = nearbyInteractables.FirstOrDefault().GetComponent<IRepairable>();
        if (repairable != null) HandleRepairing(repairable);

        float distanceToInteractable(GameObject enterableGameObjet)
        {
            return Vector3.Distance(this.gameObject.transform.position, enterableGameObjet.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ICollectable otherAsCollectable = other.GetComponent<ICollectable>();
        if (otherAsCollectable != null)
        {
            this.scrap.Collect(otherAsCollectable.Collect());
            return;
        }


        // TODO This adds duplicates of everything that has a matching interface.
        foreach (Type supportedInterface in supportedInteractionInterfaces)
        {
            if (other.gameObject.GetComponent(supportedInterface) != null)
            {
                nearbyInteractables.Add(other.gameObject);
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearbyInteractables.Contains(other.gameObject))
        {
            nearbyInteractables.Remove(other.gameObject);
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

    private void HandleRepairing(IRepairable repairable)
    {
        float repairAmountNeeded = repairable.GetRepairAmountNeeded();
        // repair as much as we can afford or the total amount needed to get to full health
        int amountToRepair = (int)Mathf.Min(scrap.Amount, repairAmountNeeded);
        scrap.Spend(amountToRepair);
        repairable.RepairAmount(amountToRepair);
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

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public bool IsValidTarget()
    {
        return true;
    }

    public TargetTypes GetTargetType()
    {
        return TargetTypes.Friendly;
    }

    public void Kill()
    {
        // TODO kill player and reload scene
        //   This should probably be handled by the gamemanager. Can be used by main base kill as well.
        Debug.Log("You died.");
    }

    public void RepairFully()
    {
        health.Heal();
    }

    public void RepairAmount(float amount)
    {
        health.Heal(amount);
    }

    public float GetRepairAmountNeeded()
    {
        return health.MaxHealth - health.CurrentHealth;
    }
}
