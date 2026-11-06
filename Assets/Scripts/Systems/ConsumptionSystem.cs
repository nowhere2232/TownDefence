using System.Collections;
using UnityEngine;

public class ConsumptionSystem : MonoBehaviour
{
    public GameConfig Config;
    public ResourceManager Resources;
    public UnitRegistry Registry;

    void Start()
    {
        StartCoroutine(ConsumeRoutine());
    }

    private IEnumerator ConsumeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Config.ConsumptionTickSeconds);

            var warriors = Registry.GetAliveUnits(UnitType.Warrior);
            foreach (var w in warriors)
            {
                if (w.IsDead) 
                    continue;
                int consumed = w.TickConsumption(Resources.Wheat);
                if (consumed > 0)
                    Resources.TrySpend(consumed);
            }
        }
    }
}
