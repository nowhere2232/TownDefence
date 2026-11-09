using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedControl : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private float[] _speeds = { 1f, 2f, 3f };

    private int _index;

    void Start()
    {
        _button.onClick.AddListener(NextSpeed);
        UpdateLabel();
    }

    private void NextSpeed()
    {
        _index = (_index + 1) % _speeds.Length;
        Time.timeScale = _speeds[_index];
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        _label.text = $"{_speeds[_index]:0}x";
    }
}
