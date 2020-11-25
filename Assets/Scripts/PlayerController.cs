using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable, IKillable, IYeetable
{
    [SerializeField]
    private PlayerMover mover;
    [SerializeField]
    private Health health;
    public event EventHandler<GameObject> OnDeath;
    private List<(IEnterable enterable, GameObject gameObject)> nearbyEnterables = new List<(IEnterable, GameObject)>();
    private GameObject playerCameraGameObject;
    private bool isInIEnterable => enterableWeAreInsideOf != null;
    private IEnterable enterableWeAreInsideOf;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        health.Initialize(this.gameObject);
        mover.Initialize(this.transform);
        playerCameraGameObject = this.gameObject.GetComponentInChildren<Camera>().gameObject;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // use the movement direction to set the animation move variable
        Vector2 movement = mover.HandleMovement();
        animator.SetFloat("MoveSpeed", movement.magnitude);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            YEET();
        }
        if (InteractButtonPressed())
        {
            TryToInteract();
        }
    }

    public void Kill()
    {
        health.Kill();
        OnDeath?.Invoke(this, this.gameObject);
        // TODO is this a race condition?
        // Destroying a game object makes the unity engine return true if we run a gameObject == null check
        // We use the enemy death in the wave spawner to remove the enemy from the list of spawned(and still alive) enemies
        // If some subscriber to the OnDeath event cheked for null we might intruduce a bug.
        // Can we delay destruction long enough for the callbacks to happen?
        // Or, could we make it the subscirbers problem, meaning subscribers to OnDeath know not to check for null equality?
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public void YEET()
    {
        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = this.gameObject.AddComponent<Rigidbody>();
        }
        rb.velocity = Vector3.up * 50;
        FindObjectOfType<AudioManager>().PlayOneShot("yeet");
    }

    private bool InteractButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    private void TryToInteract()
    {
        nearbyEnterables.Sort((x,y) => distanceToEnterable(x.gameObject).CompareTo(distanceToEnterable(y.gameObject)));
        IEnterable nearestEnterable = nearbyEnterables.FirstOrDefault().enterable;
        if (nearestEnterable != null) HandleIEnterableInteraction(nearestEnterable);

        float distanceToEnterable(GameObject enterableGameObjet)
        {
            return Vector3.Distance(this.gameObject.transform.position, enterableGameObjet.transform.position);
        }
    }

    private void HandleIEnterableInteraction(IEnterable enterable)
    {
        if (isInIEnterable)
        {
            ExitIEnterable(enterable);
        }
        else
        {
            EnterIEnterable(enterable);
        }
    }

    private void EnterIEnterable(IEnterable enterable)
    {
        enterableWeAreInsideOf = enterable;
        mover.Enabled = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        playerCameraGameObject.SetActive(false);
        enterable.Enter(this.gameObject);
    }

    private void ExitIEnterable(IEnterable enterable)
    {
        enterableWeAreInsideOf = null;
        mover.Enabled = true;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        playerCameraGameObject.SetActive(true);
        enterable.Exit(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        IEnterable enterable = other.gameObject.GetComponent<IEnterable>();
        if (enterable != null) nearbyEnterables.Add((enterable, other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        // If ware are in an enterable we do not want to change the enterableWeAreInsideOf
        if (isInIEnterable) return;
        if (other == null) return;
        IEnterable enterable = other.gameObject.GetComponent<IEnterable>();
        if (enterable != null) nearbyEnterables.Remove((enterable, other.gameObject));
    }
}
