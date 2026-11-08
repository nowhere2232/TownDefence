using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    [SerializeField] private GameConfig _config;
    [SerializeField] private EnemyConfig _enemyConfig;

    [Header("Panel")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private bool _wasPaused;

    void Start()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
        _panel.SetActive(false);
    }

    private void Open()
    {
        BuildText();
        _panel.SetActive(true);
        _wasPaused = Time.timeScale == 0f;
        Time.timeScale = 0f;
    }

    private void Close()
    {
        _panel.SetActive(false);
        if (!_wasPaused)
            Time.timeScale = 1f;
    }

    private void BuildText()
    {
        var sb = new StringBuilder();

        sb.AppendLine("<b>Воин</b>");
        AppendUnitStats(sb, _config.WarriorConfig);
        sb.AppendLine();

        sb.AppendLine("<b>Крестьянин</b>");
        AppendUnitStats(sb, _config.PeasantConfig);
        sb.AppendLine();

        sb.AppendLine("<b>Враг</b>");
        sb.AppendLine($"HP: {_enemyConfig.MaxHp}");
        sb.AppendLine($"Урон: {_enemyConfig.Damage}");
        sb.AppendLine($"Скорость атаки: {1f / _enemyConfig.AttackCooldown:F1}/сек");

        _text.text = sb.ToString();
    }

    private void AppendUnitStats(StringBuilder sb, UnitConfig cfg)
    {
        sb.AppendLine($"HP: {cfg.MaxHp}");
        sb.AppendLine($"Урон: {cfg.Damage}");
        sb.AppendLine($"Скорость атаки: {1f / cfg.AttackCooldown:F1}");
        sb.AppendLine($"Стоимость: {cfg.HireCost} пшеницы");
        sb.AppendLine($"Время найма: {cfg.HireTime} сек");

        if (cfg.ConsumesFood)
            sb.AppendLine($"Потребляет: {cfg.FoodPerTick} пшеницы / {_config.ConsumptionTickSeconds} сек");
    }
}
