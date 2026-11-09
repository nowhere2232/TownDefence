using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public GameConfig Config;
    public UnitRegistry Registry;

    [Header("Prefabs")]
    public GameObject WarriorPrefab;
    public GameObject PeasantPrefab;
    public GameObject EnemyPrefab;

    public Unit SpawnUnit(UnitType type)
    {
        var prefab = type == UnitType.Warrior ? WarriorPrefab : PeasantPrefab;
        Vector3 basePos = type == UnitType.Warrior
            ? Config.WarriorSpawnPoint
            : Config.PeasantSpawnPoint;

        var go = Instantiate(prefab, basePos + RandomJitter(), Quaternion.identity);
        var unit = go.GetComponent<Unit>();
        Registry.RegisterUnit(unit);
        return unit;
    }

    public Enemy SpawnEnemy(float hpMult, float dmgMult, float spdMult, float scaleMult)
    {
        var go = Instantiate(EnemyPrefab, Config.EnemySpawnpoint + RandomJitter(), Quaternion.identity);
        var enemy = go.GetComponent<Enemy>();
        enemy.ApplyScaling(hpMult, dmgMult, spdMult);
        go.transform.localScale = Vector3.one * scaleMult;
        Registry.RegisterEnemy(enemy);
        return enemy;
    }

    private Vector3 RandomJitter()
    {
        var v = Random.insideUnitCircle * Config.SpawnJitterRadius;
        return new Vector3(v.x, v.y, 0f);
    }
}
