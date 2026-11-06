using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName ="Game/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    [Header("Identity")]
    public UnitType Type;

    [Header("Combat")]
    public float MaxHp = 100f;
    public float Damage = 10f;
    public float AttackCooldown = 1f;
    public float AttackRange = 0.5f; // melee
    public float MoveSpeed = 1.5f;

    [Header("Hiring")]
    public int HireCost = 10;
    public float HireTime = 3f;

    [Header("Food")] // only uses if ConsumesFood = true
    public bool ConsumesFood = false;
    public float MaxSatiety = 100f;
    public float FoodPerTick = 5f;
    public float SatietyPerMeal = 20f;
    public float SatietyDrainOnHunger = 10f;
}