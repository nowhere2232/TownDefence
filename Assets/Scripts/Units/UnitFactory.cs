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

    public Enemy SpawnEnemy(int waveIndex, EnemyConfig overrideConfig = null)
    {
        var go = Instantiate(EnemyPrefab, Config.EnemySpawnpoint + RandomJitter(), Quaternion.identity);
        var enemy = go.GetComponent<Enemy>();
        if (overrideConfig != null)
        {
            enemy.Config = overrideConfig;
        }

        float hpMult = 1f + (waveIndex - 1) * enemy.Config.HpScalePerWave;
        float dmgMult = 1f + (waveIndex - 1) * enemy.Config.DamageScalePerWave;
        float spdMult = 1f + (waveIndex - 1) * enemy.Config.SpeedScalePerWave;
        enemy.ApplyScaling(hpMult, dmgMult, spdMult);

        Registry.RegisterEnemy(enemy);
        return enemy;
    }

    private Vector3 RandomJitter()
    {
        var v = Random.insideUnitCircle * Config.SpawnJitterRadius;
        return new Vector3(v.x, v.y, 0f);
    }
}
