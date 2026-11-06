using System.Collections;
using UnityEngine;

public class ProductionSystem : MonoBehaviour
{
    public GameConfig Config;
    public ResourceManager Resources;
    public UnitRegistry Registry;

    void Start()
    {
        StartCoroutine(ProduceRoutine());
    }

    private IEnumerator ProduceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Config.ProductionTickSeconds);

            int peasants = Registry.GetAliveUnits(UnitType.Peasant).Count;
            if (peasants > 0)
                Resources.AddWheat(peasants * Config.ProductionPerPeasantPerTick);
        }
    }
}