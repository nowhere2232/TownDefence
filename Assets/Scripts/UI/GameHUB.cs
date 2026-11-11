using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUB : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private ResourceManager _resources;
    [SerializeField] private UnitRegistry _registry;
    [SerializeField] private HiringSystem _hiring;
    [SerializeField] private WaveSystem _waves;
    [SerializeField] private GameManager _gameManager;
	[SerializeField] private RandomEventSystem _events;

    [Header("Texts")]
    [SerializeField] private TMP_Text _wheatText;
    [SerializeField] private TMP_Text _warriorsText;
    [SerializeField] private TMP_Text _peasantsText;
    [SerializeField] private TMP_Text _enemiesText;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _waveTimerText;
    [SerializeField] private TMP_Text _nextWaveInfoText;
	[SerializeField] private TMP_Text _eventText;

    [Header("Buttons")]
    [SerializeField] private Button _hireWarriorButton;
    [SerializeField] private Button _hirePeasantButton;
    [SerializeField] private Button _pauseButton;
	
	private Coroutine _eventNotifyRoutine;

    void Start()
    {
        _resources.OnWheatChanged += HandleWheatChanged;
        _registry.OnUnitsChanged += HandleUnitsChanged;
        _registry.OnEnemiesChanged += HandleEnemiesChanged;
        _hiring.OnQueuesChanged += HandleQueuesChanged;
        _waves.OnWaveStarted += HandleWaveStarted;
        _waves.OnWaveCleared += HandleWaveCleared;
        _gameManager.OnStateChanged += HandleStateChanged;
		_events.OnEventTriggered += HandleRandomEvent;

        _hireWarriorButton.onClick.AddListener(OnHireWarriorClicked);
        _hirePeasantButton.onClick.AddListener(OnHirePeasantClicked);
        _pauseButton.onClick.AddListener(OnPauseClicked);

        HandleWheatChanged(_resources.Wheat);
        HandleUnitsChanged();
        HandleEnemiesChanged();
        UpdateButtonStates();
        _waveText.text = $"Волна: 0 / {_config.TotalWaves}";
        if (_nextWaveInfoText != null)
            _nextWaveInfoText.text = "";
    }

    void Update()
    {
        if (_waves.NextWaveTime > 0f)
        {
            float remaining = _waves.NextWaveTime - Time.time;
            _waveTimerText.text = remaining > 0f
                ? $"До волны: {remaining:F1}"
                : "";

            int next = _waves.CurrentWave + 1;
            if (next <= _config.TotalWaves)
            {
                var preview = _waves.GetWavePreview(next);
                _nextWaveInfoText.text = preview.IsBoss
                    ? $"Босс"
                    : $"Следующая: {preview.EnemyCount} врагов";
            }
        }
        else
        {
            _waveTimerText.text = "";
            if (_waves.CurrentWave >= 1 && _waves.CurrentWave <= _config.TotalWaves)
            {
                var preview = _waves.GetWavePreview(_waves.CurrentWave);
                _nextWaveInfoText.text = preview.IsBoss
                    ? $"Босс"
                    : $"Волна {preview.WaveNumber} | {preview.EnemyCount} врагов";
            }
        }
    }

    private void HandleWheatChanged(int wheat)
    {
        _wheatText.text = $"Пшеница: {wheat}";
        UpdateButtonStates();
    }

    private void HandleUnitsChanged()
    {
        int w = _registry.GetAliveUnits(UnitType.Warrior).Count;
        int p = _registry.GetAliveUnits(UnitType.Peasant).Count;
        _warriorsText.text = $"Воины: {w} (+{_hiring.GetQueueCount(UnitType.Warrior)})";
        _peasantsText.text = $"Крестьяне: {p} (+{_hiring.GetQueueCount(UnitType.Peasant)})";
        UpdateButtonStates();
    }

    private void HandleEnemiesChanged()
    {
        int e = _registry.GetAliveEnemies().Count;
        _enemiesText.text = $"Врагов: {e}";
    }

    private void HandleQueuesChanged()
    {
        HandleUnitsChanged();
    }

    private void HandleWaveStarted(int current, int total)
    {
        _waveText.text = $"Волна: {current} / {total}";
    }

    private void HandleWaveCleared(int current)
    {
        // Число волн обновляется при старте новой волны
    }

    private void HandleStateChanged(GameState state)
    {
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        bool playing = _gameManager.State == GameState.Playing;
        _hireWarriorButton.interactable = playing && _hiring.CanHire(UnitType.Warrior);
        _hirePeasantButton.interactable = playing && _hiring.CanHire(UnitType.Peasant);
        _pauseButton.interactable = playing;
    }
	
	private void HandleRandomEvent(RandomEventConfig evt)
	{
		if (_eventNotifyRoutine != null)
			StopCoroutine(_eventNotifyRoutine);
		_eventNotifyRoutine = StartCoroutine(NotifyRoutine(evt));
	}
	
	private System.Collections.IEnumerator NotifyRoutine(RandomEventConfig evt)
	{
		string sign = IsPositive(evt.Type) ? "+" : "-";
		_eventText.text = $"{sign} {evt.DisplayName}";
		_eventText.color = IsPositive(evt.Type) ? Color.black : new Color(0.55f, 0f, 0f);
		
		float t = 0f;
		while (t < 0.3f)
		{
			t += Time.unscaledDeltaTime;
			var c = _eventText.color;
			c.a = Mathf.Lerp(0f, 1f, t / 0.3f);
			_eventText.color = c;
			yield return null;
		}
		
		yield return new WaitForSecondsRealtime(2.5f);
		
		t = 0f;
		while (t < 0.5f)
		{
			t += Time.unscaledDeltaTime;
			var c = _eventText.color;
			c.a = Mathf.Lerp(1f, 0f, t / 0.5f);
			_eventText.color = c;
			yield return null;
		}
		
		_eventNotifyRoutine = null;
	}
	
	private bool IsPositive(EventType type) => type == EventType.Harvest || type == EventType.Caravan;

    private void OnHireWarriorClicked() => _hiring.TryHire(UnitType.Warrior);
    private void OnHirePeasantClicked() => _hiring.TryHire(UnitType.Peasant);
    private void OnPauseClicked() => _gameManager.Pause();
}