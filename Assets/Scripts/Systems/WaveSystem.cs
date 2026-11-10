using System;
using System.Collections;
using UnityEngine;

public struct WavePreview
{
    public int WaveNumber;
    public int EnemyCount;
    public bool IsBoss;
    public float HpMult;
    public float DamageMult;
    public float SpeedMult;
    public float ScaleMult;
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private EnemyConfig _enemyConfig;
    [SerializeField] private UnitFactory _factory;
    [SerializeField] private GameManager _gameManager;

    public int CurrentWave { get; private set; }
    public float NextWaveTime { get; private set; } = -1f;
    public event Action<int, int> OnWaveStarted;
    public event Action<int> OnWaveCleared;

    private int _aliveEnemiesThisWave;

    void Start() => StartCoroutine(WaveLoop());

    private IEnumerator WaveLoop()
    {
        NextWaveTime = Time.time + _config.TimeBeforeFirstWave;
        yield return new WaitForSeconds(_config.TimeBeforeFirstWave);

        for (CurrentWave = 1; CurrentWave <= _config.TotalWaves; CurrentWave++)
        {
            NextWaveTime = -1f;
            OnWaveStarted?.Invoke(CurrentWave, _config.TotalWaves);

            yield return SpawnWaveRoutine(CurrentWave);

            while (_aliveEnemiesThisWave > 0) // ждёт когда все враги умрут
                yield return null;

            OnWaveCleared?.Invoke(CurrentWave);

            if (CurrentWave >= _config.TotalWaves)
            {
                _gameManager.EndGame(GameState.Victory);
                yield break;
            }

            if (CurrentWave % _config.BossWaveInterval == 0)
            {
                _gameManager.EnterMilestone();
                while (_gameManager.State == GameState.Milestone)
                    yield return null;
            }

            NextWaveTime = Time.time + _config.TimeBetweenWaves;
            yield return new WaitForSeconds(_config.TimeBetweenWaves);
        }
    }

    private IEnumerator SpawnWaveRoutine(int waveIndex)
    {
        var preview = GetWavePreview(waveIndex);
        int count = preview.EnemyCount;
        if (count <= 0) 
            yield break;

        float interval = _config.WaveSpawnDuration / count;
        _aliveEnemiesThisWave = 0;

        for (int i = 0; i < count; i++)
        {
            var enemy = _factory.SpawnEnemy(preview.HpMult, preview.DamageMult, preview.SpeedMult, preview.ScaleMult);
            _aliveEnemiesThisWave++;
            enemy.OnDied += HandleEnemyDied;

            if (i < count - 1)
                yield return new WaitForSeconds(interval);
        }
    }

    private void HandleEnemyDied(Damageable dead)
    {
        _aliveEnemiesThisWave = Mathf.Max(0, _aliveEnemiesThisWave - 1);
    }

    public WavePreview GetWavePreview(int waveIndex)
    {
        if (waveIndex < 1 || waveIndex > _config.TotalWaves)
            return default;

        bool isBoss = waveIndex % _config.BossWaveInterval == 0;
        int count = isBoss ? _config.BossCount : _config.GetEnemyCount(waveIndex - 1);

        float hpMult = 1f + (waveIndex - 1) * _enemyConfig.HpScalePerWave;
        float dmgMult = 1f + (waveIndex - 1) * _enemyConfig.DamageScalePerWave;
        float spdMult = 1f + (waveIndex - 1) * _enemyConfig.SpeedScalePerWave;
        float scaleMult = 1f;

        if (isBoss)
        {
            hpMult *= _config.BossHpMulti;
            dmgMult *= _config.BossDamageMulti;
            spdMult *= _config.BossSpeedMulti;
            scaleMult = _config.BossScaleMulti;
        }

        return new WavePreview
        {
            WaveNumber = waveIndex,
            EnemyCount = count,
            IsBoss = isBoss,
            HpMult = hpMult,
            DamageMult = dmgMult,
            SpeedMult = spdMult,
            ScaleMult = scaleMult
        };
    }
}
