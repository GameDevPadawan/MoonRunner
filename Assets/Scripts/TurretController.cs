using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private TurretTargetting targetting;
    public Vector3 rotation;

    void Awake()
    {
        targetting = new TurretTargetting(this.transform.GetChild(0).GetChild(8));
    }

    // Update is called once per frame
    void Update()
    {
        targetting.HandleAiming(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.ReceiveAgro(this.gameObject);
            if (targetting != null)
            {
                targetting.OnTriggerEnter(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.LoseAgro();
            if (targetting != null)
            {
                targetting.OnTriggerExit(other);
            }
        }
    }
}
