using UnityEngine;
using UnityEngine.InputSystem;

public class DevTest : MonoBehaviour
{
    public HiringSystem Hiring;
    public ResourceManager Resources;
    public UnitRegistry Registry;
    public WaveSystem WaveSystem;

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.digit1Key.wasPressedThisFrame)
            Hiring.TryHire(UnitType.Warrior);

        if (keyboard.digit2Key.wasPressedThisFrame)
            Hiring.TryHire(UnitType.Peasant);

        if (keyboard.spaceKey.wasPressedThisFrame)
            LogState();
    }

    private void LogState()
    {
        int warriors = Registry.GetAliveUnits(UnitType.Warrior).Count;
        int peasants = Registry.GetAliveUnits(UnitType.Peasant).Count;
        int enemies = Registry.GetAliveEnemies().Count;

        string waveInfo = WaveSystem != null
            ? $"wave={WaveSystem.CurrentWave}"
            : "wave=n/a";

        print(
            $"wheat={Resources.Wheat} | " +
            $"warriors+{warriors} (queue={Hiring.GetQueueCount(UnitType.Warrior)}) | " +
            $"peasants+{peasants} (queue={Hiring.GetQueueCount(UnitType.Peasant)}) | " +
            $"enemies={enemies} | {waveInfo}"
        );
    }
}
