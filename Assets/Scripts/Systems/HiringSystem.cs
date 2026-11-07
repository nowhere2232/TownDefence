using System;
using UnityEngine;

public class HiringSystem : MonoBehaviour
{
    public GameConfig Config;
    public ResourceManager Resources;
    public UnitFactory Factory;

    private int _warriorQueue;
    private int _peasantQueue;
    private float _warriorTimer;
    private float _peasantTimer;
    private bool _isHiringWarrior;
    private bool _isHiringPeasant;

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
            if (!_isHiringWarrior)
            {
                _isHiringWarrior = true;
                _warriorTimer = cfg.HireTime;
            }
        }
        else
        {
            _peasantQueue++;
            if (!_isHiringPeasant)
            {
                _isHiringPeasant = true;
                _peasantTimer = cfg.HireTime;
            }
        }

        OnQueuesChanged?.Invoke();
    }

    void Update()
    {
        if (_isHiringWarrior)
        {
            _warriorTimer -= Time.deltaTime;
            if (_warriorTimer <= 0f)
            {
                _warriorQueue--;
                Factory.SpawnUnit(UnitType.Warrior);
                OnQueuesChanged?.Invoke();

                if (_warriorQueue > 0)
                    _warriorTimer = Config.WarriorConfig.HireTime;
                else
                    _isHiringWarrior = false;
            }
        }

        if (_isHiringPeasant)
        {
            _peasantTimer -= Time.deltaTime;
            if (_peasantTimer <= 0f)
            {
                _peasantQueue--;
                Factory.SpawnUnit(UnitType.Peasant);
                OnQueuesChanged?.Invoke();

                if (_peasantQueue > 0)
                    _peasantTimer = Config.PeasantConfig.HireTime;
                else
                    _isHiringPeasant = false;
            }
        }
    }

    private UnitConfig GetConfig(UnitType type) => type == UnitType.Warrior ? Config.WarriorConfig : Config.PeasantConfig;
}