using System;
using System.Collections;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private UnitFactory _factory;
    [SerializeField] private GameManager _gameManager;

    public int CurrentWave { get; private set; }
    public float NextWaveTime { get; private set; } = -1f;
    public event Action<int, int> OnWaveStarted;
    public event Action<int> OnWaveCleared;

    private int _aliveEnemiesThisWave;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

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

            NextWaveTime = Time.time + _config.TimeBetweenWaves;
            yield return new WaitForSeconds(_config.TimeBetweenWaves);
        }
    }

    private IEnumerator SpawnWaveRoutine(int waveIndex)
    {
        int count = _config.EnemiesPerWave[waveIndex - 1];
        if (count <= 0) 
            yield break;

        float interval = _config.WaveSpawnDuration / count;
        _aliveEnemiesThisWave = 0;

        for (int i = 0; i < count; i++)
        {
            var enemy = _factory.SpawnEnemy();
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
}
