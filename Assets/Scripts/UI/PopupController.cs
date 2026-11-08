using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private WaveSystem _waves;

    [Header("Panels")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _defeatPanel;

    [Header("Victory stats")]
    [SerializeField] private TMP_Text _victoryStatsText;
    [Header("Defeat stats")]
    [SerializeField] private TMP_Text _defeatStatsText;

    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartPauseButton;
    [SerializeField] private Button _restartVictoryButton;
    [SerializeField] private Button _restartDefeatButton;

    void Start()
    {
        _gameManager.OnStateChanged += HandleStateChanged;

        _startButton.onClick.AddListener(OnStartClicked);
        _resumeButton.onClick.AddListener(() => _gameManager.Resume());
        _restartPauseButton.onClick.AddListener(RestartGame);
        _restartVictoryButton.onClick.AddListener(RestartGame);
        _restartDefeatButton.onClick.AddListener(RestartGame);

        HandleStateChanged(_gameManager.State);
    }

    private void OnStartClicked()
    {
        _gameManager.StartGame();
    }

    private void HandleStateChanged(GameState state)
    {
        HideAll();

        switch (state)
        {
            case GameState.Start:
                _startPanel.SetActive(true); 
                break;

            case GameState.Paused:
                _pausePanel.SetActive(true); 
                break;

            case GameState.Victory:
                _victoryPanel.SetActive(true);
                if (_victoryStatsText != null)
                    _victoryStatsText.text = "Все волны отбиты!";
                break;

            case GameState.Defeat:
                _defeatPanel.SetActive(true);
                if (_defeatStatsText != null)
                    _defeatStatsText.text = $"Деревня пала на волне {GetCurrentWave()}";
                break;

        }
    }

    private void HideAll()
    {
        _startPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _victoryPanel.SetActive(false);
        _defeatPanel.SetActive(false);
    }

    private int GetCurrentWave()
    {
        return _waves != null ? _waves.CurrentWave : 0;
    }

    private void RestartGame()
    {
        _gameManager.Restart();
    }
}
