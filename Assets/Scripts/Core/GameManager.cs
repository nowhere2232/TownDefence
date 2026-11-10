using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameConfig Config;
    public ResourceManager Resources;
    public UnitRegistry Registry;
    public Village Village;

    public GameState State { get; private set; } = GameState.Start;
    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        State = GameState.Start;
        Time.timeScale = 0f;
        Resources.Init(Config.StartingWheat);
    }

    void Start()
    {
        Village.OnAllStructuresDestroyed += () => EndGame(GameState.Defeat);
    }

    public void StartGame()
    {
        if (State != GameState.Start)
            return;
        SetState(GameState.Playing);
    }

    public void Pause()
    {
        if (State != GameState.Playing)
            return;
        SetState(GameState.Paused);
    }

    public void EnterMilestone()
    {
        if (State != GameState.Playing)
            return;
        SetState(GameState.Milestone);
    }

    public void ContinueAfterMilestone()
    {
        if (State != GameState.Milestone) 
            return;
        SetState(GameState.Playing);
    }

    public void Resume()
    {
        if (State != GameState.Paused)
            return;
        SetState(GameState.Playing);
    }

    public void EndGame(GameState result)
    {
        if (State == GameState.Victory || State == GameState.Defeat)
            return;
        SetState(result);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetState(GameState s)
    {
        State = s;
        OnStateChanged?.Invoke(s);
    }
}
