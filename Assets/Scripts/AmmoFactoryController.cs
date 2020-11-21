using System;
using System.Linq;
using UnityEngine;

public class AmmoFactoryController : MonoBehaviour
{
    /// <summary>
    /// We are using a collider and <see cref="targetToReload"/> to track entrence and exit of <see cref="IReloadable"/>s.
    /// This is probably overkill since there is only one player, but it's already set up so I will use it.
    /// </summary>
    IReloadable targetToReload;
    private bool weHaveReloadableTarget => targetToReload != null;
    /// <summary>
    /// The number of seconds between entering, or receiving a reload, before the next reload occurs.
    /// </summary>
    [SerializeField]
    private float reloadDelay = 3;
    private float timeOfLastReload;
    private bool isTimeToReload => Time.time - timeOfLastReload > reloadDelay;
    
    void Awake()
    {
        // Ensures the object ahs the components required to function properly.
        ValidateObject();

        void ValidateObject()
        {
            // If there is more than one collider with isTrigger set to true we won't know which one is the reload zone.
            // If we don't find any we don't have a reload zone.
            var triggerBoxColliders = GetComponents<BoxCollider>().Where(x => x.isTrigger == true);
            if (triggerBoxColliders.Count() != 1)
            {
                throw new Exception(
                    $"There should be exactly one box collider with isTrigger set to true on the ammo factory. " +
                    $"We found {triggerBoxColliders.Count()}.");
            }
        }
    }


    void Update()
    {
        if (weHaveReloadableTarget && isTimeToReload)
        {
            ExecuteReload();
        }
    }

    private void ExecuteReload()
    {
        if (targetToReload.NeedsReload())
        {
            targetToReload.ReloadAmount(1); 
        }
        timeOfLastReload = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        IReloadable newReloadableTarget = other.GetComponent<IReloadable>();
        if (newReloadableTarget != null)
        {
            // Always keep the most recent IReloadable as the current target.
            targetToReload = newReloadableTarget;
            // Set the time of last reload to 
            timeOfLastReload = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IReloadable newReloadableTarget = other.GetComponent<IReloadable>(); ;
        if (newReloadableTarget != null)
        {
            // If the IReloadable leaving the collider is the current reload target, null the current target
            if (newReloadableTarget.Equals(targetToReload))
            {
                targetToReload = null;
            }
        }
    }
}
