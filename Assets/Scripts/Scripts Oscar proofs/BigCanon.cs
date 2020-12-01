using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BigCanon : MonoBehaviour, IRepairable
{

    [SerializeField]
    private List<GameObject> cannonAssemblyStates;
    private Dictionary<float, GameObject> assemblyLevelByHealthPercentage = new Dictionary<float, GameObject>();
    private GameObject currentlyEnabledObject;
    public Health health;
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform barrelTip;
    private bool IsFinished;
    [SerializeField]
    private bool debugWinGame;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        IsFinished = false;
        cannonAssemblyStates.ForEach(x => x.SetActive(false));
        // create a dictionary of all assembly states. 
        //   Use the index of the state to create the health level associated with each state
        for (int i = 0; i < cannonAssemblyStates.Count; i++)
        {
            if (i == 0)
            {
                // We want the lowest assembly level to be zero health
                assemblyLevelByHealthPercentage.Add(i, cannonAssemblyStates[i]);
            }
            else
            {
                // Create the required health levels as if index 0 did not exist.
                //   This is why we subtract 1 from cannonAssemblyStates.Count
                assemblyLevelByHealthPercentage.Add(
                    ((float)i / ((float)cannonAssemblyStates.Count - 1)) * health.MaxHealth,
                    cannonAssemblyStates[i]);
            }
        }

        health.Initialize(this.gameObject);
        // We want the cannon to start at 0 health.
        health.TakeDamage(health.MaxHealth);
        health.OnHealed += Health_OnHealed;
        Health_OnHealed();
    }

    /// <summary>
    /// Handles checking and updating the assembly state.
    /// This is cheaper than checking in update.
    /// </summary>
    private void Health_OnHealed()
    {
        GameObject highestAssemblyStateAllowed = getHighestAllowedAssemblyState();
        if (currentlyEnabledObject != highestAssemblyStateAllowed)
        {
            currentlyEnabledObject = highestAssemblyStateAllowed;
            EnableAssemblyLevel(currentlyEnabledObject);
        }
    }

    private GameObject getHighestAllowedAssemblyState()
    {
        var statesAllowedByCurrentHealth = assemblyLevelByHealthPercentage.Where(x => x.Key <= health.CurrentHealth).ToList();
        GameObject highestAssemblyStateAllowed = statesAllowedByCurrentHealth.OrderByDescending(x => x.Key).FirstOrDefault().Value;
        return highestAssemblyStateAllowed;
    }

    private void EnableAssemblyLevel(GameObject currentlyEnabledObject)
    {
        cannonAssemblyStates.ForEach(x => x.SetActive(false));
        currentlyEnabledObject.SetActive(true);
    }

    void Update()
    {
        if ((health.IsFullHealth || debugWinGame) && IsFinished== false)
        {
            if (debugWinGame)
            {
                GameObject highestAssemblyStateAllowed = assemblyLevelByHealthPercentage.OrderByDescending(x => x.Key).FirstOrDefault().Value;
                EnableAssemblyLevel(highestAssemblyStateAllowed);
            }
            // end game
            Debug.Log("You win!");
            IsFinished = true;
            StartCoroutine(GameObject.FindObjectOfType<EndGameScript>().PlayEndScene(this));
        }
    }

    public void DrawLine(Vector3 targetPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.widthMultiplier = 5;
        lineRenderer.SetPositions(new Vector3[] { barrelTip.position, targetPos });
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

    public float GetCompleteRatio()
    {
        return health.CurrentHealth / health.MaxHealth;
    }
}
