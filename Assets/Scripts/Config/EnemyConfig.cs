using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Identity")]
    public string DisplayName = "Enemy";

    [Header("Combat")]
    public float MaxHp = 50f;
    public float Damage = 5f;
    public float AttackCooldown = 1.5f;
    public float AttackRange = 0.5f;
    public float MoveSpeed = 1f;

    [Header("Wave scaling")]
    public float HpScalePerWave = 0.08f; // +8%
    public float DamageScalePerWave = 0.05f; // +5%
    public float SpeedScalePerWave = 0.005f; // +0.5%
}