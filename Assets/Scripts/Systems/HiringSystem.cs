using System;
using System.Collections;
using UnityEngine;

public class HiringSystem : MonoBehaviour
{
    public GameConfig Config;
    public ResourceManager Resources;
    public UnitFactory Factory;

    private int _warriorQueue;
    private int _peasantQueue;
    private Coroutine _warriorRoutine;
    private Coroutine _peasantRoutine;

    public event Action OnQueuesChanged;

    public int GetQueueCount(UnitType type) => type == UnitType.Warrior ? _warriorQueue : _peasantQueue;

    public bool CanHire(UnitType type)
    {
        if (GetQueueCount(type) >= Config.MaxQueueLength)
            return false;
        var cfg = GetConfig(type);
        if (!Resources.CanAfford(cfg.HireCost))
            return false;
        return true;
    }

    public void TryHire(UnitType type)
    {
        if (!CanHire(type))
            return;

        var cfg = GetConfig(type);
        Resources.TrySpend(cfg.HireCost);

        if (type == UnitType.Warrior)
        {
            _warriorQueue++;
            if (_warriorRoutine == null)
                _warriorRoutine = StartCoroutine(HireRoutine(UnitType.Warrior));
        }
        else
        {
            _peasantQueue++;
            if (_peasantRoutine == null)
                _peasantRoutine = StartCoroutine(HireRoutine(UnitType.Peasant));
        }

        OnQueuesChanged?.Invoke();
    }

    private IEnumerator HireRoutine(UnitType type)
    {
        var cfg = GetConfig(type);

        while (GetQueueCount(type) > 0)
        {
            yield return new WaitForSeconds(cfg.HireTime);

            if (type == UnitType.Warrior)
                _warriorQueue--;
            else
                _peasantQueue--;

            Factory.SpawnUnit(type);
            OnQueuesChanged?.Invoke();
        }

        if (type == UnitType.Warrior)
            _warriorRoutine = null;
        else
            _warriorRoutine = null;
    }

    private UnitConfig GetConfig(UnitType type) => type == UnitType.Warrior ? Config.WarriorConfig : Config.PeasantConfig;
}