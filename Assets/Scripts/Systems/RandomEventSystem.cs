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
    [SerializeField] private RandomEventConfig[] _events;

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
		foreach (var e in _events) total += e.Weight;
		if (total <= 0)
			return null;
		
		float roll = UnityEngine.Random.value * total;
		float acc = 0f;
		foreach (var e in _events)
		{
			acc += e.Weight;
			if (roll <= acc)
				return e;
		}
		return _events[_events.Length - 1];
    }
	
	private void Execute(RandomEventConfig evt)
	{
		switch (evt.Type)
		{
			case EventType.Harvest:
			{
				int gain = Mathf.RoundToInt(_resources.Wheat * evt.PercentOfWheat);
				gain = Mathf.Max(gain, evt.Magnitude);
				_resources.AddWheat(gain);
				break;
			}
			case EventType.Caravan:
			{
				_factory.SpawnUnit(UnitType.Peasant);
				break;
			}
			case EventType.Famine:
			{
				int loss = Mathf.RoundToInt(_resources.Wheat * evt.PercentOfWheat);
				loss = Mathf.Max(loss, evt.Magnitude);
				_resources.TrySpend(Mathf.Min(loss, _resources.Wheat));
				break;
			}
			case EventType.Plague:
			{
				var peasants = _registry.GetAliveUnits(UnitType.Peasant);
				int kill = Mathf.Min(evt.Magnitude, peasants.Count);
				for (int i = 0; i < kill; i++)
					peasants[i].TakeDamage(99999f);
				break;
			}
			case EventType.Rats:
			{
				_resources.TrySpend(Mathf.Min(evt.Magnitude, _resources.Wheat));
				break;
			}
			case EventType.Raid:
			{
				var enemy = _factory.SpawnEnemy(1f, 1f, 1f, 1f);
				break;
			}
		}
		
		OnEventTriggered?.Invoke(evt);
	}
}
