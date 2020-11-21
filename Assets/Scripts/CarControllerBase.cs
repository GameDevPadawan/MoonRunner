using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public abstract class CarControllerBase : MonoBehaviour, IEnterable
{
    [SerializeField]
    protected CarMover mover;
    protected List<GameObject> passengers = new List<GameObject>();
    protected bool anyPassengers => passengers != null && passengers.Any();
    protected Type interactionType;
    protected List<Component> interactableComponents = new List<Component>();
    protected bool controlEnabled => anyPassengers;

    protected virtual void Start() { }
    protected virtual void Awake() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }

    public void Enter(GameObject passenger)
    {
        if (passenger == null) return;
        passenger.transform.parent = this.transform;
        passenger.transform.localPosition = this.transform.up;
        passengers.Add(passenger);
    }

    public void Exit(GameObject passenger)
    {
        if (passenger == null) return;
        passenger.transform.parent = null;
        passenger.transform.position = this.transform.position + (this.transform.right * 3);
        passengers.Remove(passenger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        Component interfaceOfInteractionType = other.gameObject.GetComponent(interactionType);
        if (interfaceOfInteractionType != null) interactableComponents.Add((interfaceOfInteractionType));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;
        Component interfaceOfInteractionType = other.gameObject.GetComponent(interactionType);
        if (interfaceOfInteractionType != null) interactableComponents.Remove((interfaceOfInteractionType));
    }
}
