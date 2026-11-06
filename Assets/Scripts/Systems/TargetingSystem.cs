using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    public UnitRegistry Registry;
    public Village Village;

    void Start()
    {
        Registry.OnUnitsChanged += Reassign;
        Registry.OnEnemiesChanged += Reassign;
    }

    private void Reassign()
    {
        foreach (var u in Registry.Units) u.Target = null;
        foreach (var e in Registry.Enemies)
        {
            e.TargetUnit = null;
            e.TargetStructure = null;
        }

        var warriors = Registry.GetAliveUnits(UnitType.Warrior);
        var peasants = Registry.GetAliveUnits(UnitType.Peasant);
        var enemies = Registry.GetAliveEnemies();

        int enemyIdx = 0;

        for (int i = 0; i < warriors.Count && enemyIdx < enemies.Count; i++)
        {
            warriors[i].Target = enemies[enemyIdx];
            enemies[enemyIdx].TargetUnit = warriors[i];
            enemyIdx++;
        }

        for (int i = 0; i + 2 < peasants.Count && enemyIdx < enemies.Count; i += 3)
        {
            var e = enemies[enemyIdx];
            peasants[i].Target = e;
            peasants[i + 1].Target = e;
            peasants[i + 2].Target = e;
            e.TargetUnit = peasants[i];
            enemyIdx++;
        }

        for (; enemyIdx < enemies.Count; enemyIdx++)
        {
            enemies[enemyIdx].TargetStructure = Village.GetNearestIntact(enemies[enemyIdx].transform.position);
        }
    }
}
