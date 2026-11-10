using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedControl : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private float[] _speeds = { 1f, 2f, 3f };
    [SerializeField] private GameManager _gameManager;

    private int _index;

    void Start()
    {
        _button.onClick.AddListener(NextSpeed);
        _gameManager.OnStateChanged += HandleStateChanged;
        HandleStateChanged(_gameManager.State);
        UpdateLabel();
    }

    private void NextSpeed()
    {
        _index = (_index + 1) % _speeds.Length;
        ApplySpeed();
        UpdateLabel();
    }

    private void HandleStateChanged(GameState state)
    {
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        Time.timeScale = _gameManager.State == GameState.Playing
            ? _speeds[_index]
            : 0f;
    }

    private void UpdateLabel()
    {
        _label.text = $"{_speeds[_index]:0}x";
    }
}
