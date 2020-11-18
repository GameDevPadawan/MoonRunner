using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairCarController : CarControllerBase
{
    public float AmmoInCar, MaxAmmoInCar;
    void Awake()
    {
        mover.Initialize(this.transform);
        base.interactionType = typeof(IRepairable);
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            mover.HandleMovement(); 
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "AmmoFactory")
        {
            AmmoInCar = MaxAmmoInCar;
        }
    }
    
}
