using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField]
    private TurretTargetting targetting;
    [SerializeField]
    private TurretShooting shooting;

    void Awake()
    {
        targetting = new TurretTargetting(this.transform.GetChild(0).GetChild(8));
        shooting = new TurretShooting();
    }

    // Update is called once per frame
    void Update()
    {
        targetting.HandleAiming();
        if (targetting.IsAimedAtTarget())
        {
            shooting.HandleShooting(targetting.GetCurrentTarget());
        }
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
