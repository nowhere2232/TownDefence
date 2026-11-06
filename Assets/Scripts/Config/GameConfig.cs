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
    public int TotalWaves = 20;
    public float TimeBeforeFirstWave = 10f;
    public float TimeBetweenWaves = 5f;
    public float WaveSpawnDuration = 10f;
    public int[] EnemiesPerWave = { 3, 5, 8, 10, 15, 18, 22, 25, 30, 40, 45, 50, 55, 60, 70, 75, 80, 90, 100, 120};

    [Header("Spawn positions")]
    public Vector3 EnemySpawnpoint = new Vector3(8f, 0f, 0f);
    public Vector3 PeasantSpawnPoint = new Vector3(-5f, -2f, 0f);
    public Vector3 WarriorSpawnPoint = new Vector3(-5f, 2f, 0f);
    public float SpawnJitterRadius = 0.5f;
}