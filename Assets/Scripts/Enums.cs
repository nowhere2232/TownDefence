public enum GameState
{
    Start,
    Playing,
    Paused,
    Milestone,
    Victory,
    Defeat
}

public enum UnitType
{
    Warrior,
    Peasant
}

public enum StructureType
{
    House,
    Barricade,
    Field
}

public enum EventType
{
    // на момент
    Harvest, Caravan, Famine, Plague, Rats, Raid,
    // на время
    GoodHarvest, Drought, Feast
}

public enum ModifierType
{
    ProductionMultiplier,
    ConsumptionMultiplier
}