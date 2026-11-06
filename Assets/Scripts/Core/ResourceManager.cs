using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int Wheat { get; private set; }
    public event Action<int> OnWheatChanged;

    public void Init(int startingWheat)
    {
        Wheat = startingWheat;
        OnWheatChanged?.Invoke(Wheat);
    }

    public void AddWheat(int amount)
    {
        if (amount <= 0)
            return;
        Wheat += amount;
        OnWheatChanged?.Invoke(Wheat);
    }

    public bool CanAfford(int amount) => Wheat >= amount;

    public bool TrySpend(int amount)
    {
        if (Wheat < amount)
            return false;
        Wheat -= amount;
        OnWheatChanged?.Invoke(Wheat);
        return true;
    }
}
