using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AmmoCarController : CarControllerBase
{
    [SerializeField]
    private int ammoBoxes;
    private bool weHaveAmmo => ammoBoxes > 0;
    void Awake()
    {
        mover.Initialize(this.transform);
        base.interactionType = typeof(IReloadable);
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            mover.HandleMovement();
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
}
