using System;
using UnityEngine;

public class Scrap
{
    public Action<int> OnScrapChanged;
    private int _amount;
    public int Amount
    {
        get => _amount;
        set {
            _amount = value;
            OnScrapChanged?.Invoke(value);
        }
    }

    public bool CanAfford(int cost) => cost <= Amount;

    public bool Spend(int cost)
    {
        if(!CanAfford(cost)) 
            return false;

        Amount -= cost;
        return true;
    }

    public void Collect(int amount)
    {
        Amount += amount;
    }
}
