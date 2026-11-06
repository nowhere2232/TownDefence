using UnityEngine;

[CreateAssetMenu(fileName = "StructureConfig", menuName = "Game/StructureConfig")]
public class StructureConfig : ScriptableObject
{
    [Header("Identity")]
    public StructureType Type;

    [Header("Combat")]
    public float MaxHp = 500f;
}