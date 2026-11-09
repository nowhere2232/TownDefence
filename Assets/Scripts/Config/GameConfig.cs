using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Unit configs")]
    public UnitConfig WarriorConfig;
    public UnitConfig PeasantConfig;

    [Header("Enemy configs")]
    public EnemyConfig[] EnemyConfigs;

    [Header("Economy")]
    public int StartingWheat = 50;
    public float ProductionTickSeconds = 2f;
    public int ProductionPerPeasantPerTick = 5;
    public float ConsumptionTickSeconds = 3f;

    [Header("Hiring")]
    public int MaxQueueLength = 10; // лимит наймов за раз

    [Header("Waves")]
    public int TotalWaves = 50;
    public float TimeBeforeFirstWave = 10f;
    public float TimeBetweenWaves = 5f;
    public float WaveSpawnDuration = 10f;

    [Header("Boss waves")]
    public int BossWaveInterval = 10;
    public int BossCount = 1;
    public float BossHpMulti = 5f;
    public float BossDamageMulti = 3f;
    public float BossSpeedMulti = 1.2f;
    public float BossScaleMulti = 1.8f;

    [Header("Wave formula")]
    public int BaseEnemyCount = 3;
    public float LinearGrowth = 2f;
    public float QuadraticGrowth = 0.3f;
    public int MaxEnemyCount = 2000;

    public int GetEnemyCount(int waveIndex)
    {
        if (waveIndex < 1)
            return 0;
        float w = waveIndex - 1;
        int count = Mathf.RoundToInt(BaseEnemyCount + w * LinearGrowth + w * w * QuadraticGrowth);
        return Mathf.Clamp(count, 1, MaxEnemyCount);
    }

    [Header("Spawn positions")]
    public Vector3 EnemySpawnpoint = new Vector3(8f, 0f, 0f);
    public Vector3 PeasantSpawnPoint = new Vector3(-5f, -2f, 0f);
    public Vector3 WarriorSpawnPoint = new Vector3(-5f, 2f, 0f);
    public float SpawnJitterRadius = 0.5f;
}