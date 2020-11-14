using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCarController : MonoBehaviour, IEnterable
{
    [SerializeField]
    private CarMover mover;
    private List<GameObject> passengers = new List<GameObject>();
    private bool anyPassengers => passengers.Count > 0;

    public void Enter(GameObject passenger)
    {
        if (passenger == null) return;
        passenger.transform.parent = this.transform;
        passenger.transform.localPosition = this.transform.up;
        passengers.Add(passenger);
        mover.Enabled = true;
    }

    public void Exit(GameObject passenger)
    {
        if (passenger == null) return;
        passenger.transform.parent = null;
        passenger.transform.position = this.transform.position + (this.transform.right * 3);
        passengers.Remove(passenger);
        if (!anyPassengers)
        {
            mover.Enabled = false; 
        }
    }

    void Awake()
    {
        mover.Initialize(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        mover.HandleMovement();
    }


}
