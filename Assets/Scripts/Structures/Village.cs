using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public List<Structure> structures = new();

    public event System.Action OnAllStructuresDestroyed;
    private bool _defeatFired;

    void Awake()
    {
        foreach (var s in structures)
            s.OnDied += _ => CheckAllDead();
    }

    private void CheckAllDead()
    {
        if (_defeatFired) 
            return;
        if (structures.Count == 0)
            return;
        if (structures.TrueForAll(s => s.IsDead))
        {
            _defeatFired = true;
            OnAllStructuresDestroyed?.Invoke();
        }
    }

    public Structure GetNearestIntact(Vector3 from)
    {
        Structure best = null;
        float bestSqr = float.MaxValue;
        foreach (var s in structures)
        {
            if (s.IsDead)
                continue;
            float d = (s.transform.position - from).sqrMagnitude;
            if (d < bestSqr) { bestSqr = d; best = s; }
        }
        return best;
    }
}
