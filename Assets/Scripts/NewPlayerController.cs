using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewPlayerController : MonoBehaviour, IReloadable, IDamageable, IKillable, IRepairable
{
    public event Action OnAmmoBoxesChanged;
    [SerializeField]
    private int ammoBoxes;
    [SerializeField]
    private int maxAmmoBoxes;
    public int MAXAmmoBoxes => maxAmmoBoxes;

    public int AmmoBoxes
    {
        get => ammoBoxes;
        set
        {
            ammoBoxes = value;
            OnAmmoBoxesChanged?.Invoke();
        }
    }

    private bool weHaveAmmo => AmmoBoxes > 0;
    private NewVehicleController mover;
    [SerializeField]
    private Health health;
    public Health Health => health;
    private List<Type> supportedInteractionInterfaces = new List<Type>();
    [SerializeField]
    private List<GameObject> nearbyInteractables = new List<GameObject>();
    public event EventHandler<GameObject> OnDeath;
    [SerializeField]
    private Scrap scrap;
    public Scrap Scrap => scrap;
    private AudioManager audioManager;

    //Óscar mods
    private AudioSource Aus;
    public AudioClip acel, brake;

    protected void Awake()
    {
        supportedInteractionInterfaces = new List<Type>() { typeof(IReloadable), typeof(IRepairable) };
        health.Initialize(this.gameObject);
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        mover = GetComponent<NewVehicleController>();
    }

    void Start()
    {
        Aus = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mover.SetInput(input);
        PlayVehicleSounds(input);
        bool spacePressed = Input.GetKey(KeyCode.Space);
        mover.Slide(spacePressed);
        //if (spacePressed) vehicleMovementScript.SetRotationToUpright();
        if (InteractPressed())
        {
            TryToInteract();
        }
        if(Input.GetKeyDown(KeyCode.W) && !Aus.isPlaying)
        {
            Aus.clip = acel;
            Aus.Play();
        }
        else if (Input.GetKeyDown(KeyCode.S) && !Aus.isPlaying)
        {
            Aus.clip = brake;
            Aus.Play();
        }
        else if (Input.GetKeyUp(KeyCode.W) && Aus.isPlaying || Input.GetKeyUp(KeyCode.S) && Aus.isPlaying)
        {
            Aus.Stop();
        }

    }

    private void PlayVehicleSounds(Vector2 input)
    {
       

        /* if (input.y > 0 )
         {
             audioManager.PlayOneShot("acceleratingVehicle");
             audioManager.Stop("brakingVehicle");
         }
         else if (input.y < 0)
         {
             audioManager.PlayOneShot("brakingVehicle");
             audioManager.Stop("acceleratingVehicle");
         }
         else
         {
             audioManager.Stop("brakingVehicle");
             audioManager.Stop("acceleratingVehicle");
         }*/
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

        IReloadable reloadable = nearbyInteractables.FirstOrDefault()?.GetComponent<IReloadable>();
        if (reloadable != null) HandleReloading(reloadable);

        IRepairable repairable = nearbyInteractables.FirstOrDefault()?.GetComponent<IRepairable>();
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
            AmmoBoxes--;
            audioManager.PlayOneShot("reload");
        }
    }

    private void HandleRepairing(IRepairable repairable)
    {
        float repairAmountNeeded = repairable.GetRepairAmountNeeded();
        // repair as much as we can afford or the total amount needed to get to full health
        int amountToRepair = (int)Mathf.Min(scrap.Amount, repairAmountNeeded);
        scrap.Spend(amountToRepair);
        repairable.RepairAmount(amountToRepair);
        if (amountToRepair > 0) audioManager.PlayOneShot("repairSound");
    }

    public void ReloadFully()
    {
        this.AmmoBoxes = maxAmmoBoxes;
    }

    public void ReloadAmount(int amount)
    {
        this.AmmoBoxes += amount;
    }

    public bool NeedsReload()
    {
        return this.AmmoBoxes < this.maxAmmoBoxes;
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
