using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private TurretTargetting targetting;
    

    void Awake()
    {
        targetting = new TurretTargetting(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.gameObject.name} entered");
        if (targetting != null)
        {
            targetting.OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"{other.gameObject.name} exited");
        if (targetting != null)
        {
            targetting.OnTriggerExit(other);
        }
    }
}
