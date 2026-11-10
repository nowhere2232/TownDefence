using UnityEngine;

[CreateAssetMenu(fileName = "randomEventConfig", menuName = "Game/RandomEventConfig")]
public class RandomEventConfig : ScriptableObject
{
    public EventType Type;
    public string DisplayName = "Событие";
    public string Description = "";
    public float Weight = 1f;
    public int Magnitude = 10;
    public float PercentOfWheat = 0f;
}