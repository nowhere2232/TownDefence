using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitRegistry : MonoBehaviour
{
    public List<Unit> Units { get; } = new();
    public List<Enemy> Enemies { get; } = new();

    public event Action OnUnitsChanged;
    public event Action OnEnemiesChanged;

    public void RegisterUnit(Unit unit)
    {
        Units.Add(unit);
        unit.OnDied += OnUnitDied;
        OnUnitsChanged?.Invoke();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
        enemy.OnDied += OnEnemyDied;
        OnEnemiesChanged?.Invoke();
    }

    private void OnUnitDied(Damageable d)
    {
        var u = (Unit)d;
        Units.Remove(u);
        OnUnitsChanged?.Invoke();
    }

    private void OnEnemyDied(Damageable d)
    {
        var e = (Enemy)d;
        Enemies.Remove(e);
        OnEnemiesChanged?.Invoke();
    }

    public List<Unit> GetAliveUnits(UnitType type)
    {
        return Units.FindAll(u => !u.IsDead && u.Config.Type == type);
    }

    public List<Enemy> GetAliveEnemies()
    {
        return Enemies.FindAll(e => !e.IsDead);
    }
}