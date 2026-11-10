using System;
using System.Collections;
using UnityEngine;

public class RandomEventSystem : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResourceManager _resources;
    [SerializeField] private UnitRegistry _registry;
    [SerializeField] private HiringSystem _hiring;
    [SerializeField] private UnitFactory _factory;
    [SerializeField] private RandomEventConfig _events;

    public event Action<RandomEventConfig> OnEventTriggered;

    private void Start() => StartCoroutine(EventLoop());

    private IEnumerator EventLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(_config.EventIntervalSeconds);

            if (_gameManager.State != GameState.Playing) // скип событий при паузе
                continue;

            if (UnityEngine.Random.value > _config.EventChance) // рандом
                continue;

            var evt = PickWeighted();
            if (evt != null)
                Execute(evt);
        }
    }

    private RandomEventConfig PickWeighted()
    {
        float total = 0f;
    }
}
