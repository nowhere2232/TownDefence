using UnityEngine;

public class Enemy : Damageable
{
    public EnemyConfig Config;
    public override float MaxHp => Config.MaxHp;

    public Unit TargetUnit { get; set; }
    public Structure TargetStructure { get; set; }

    private float _attackTimer;

    void Awake()
    {
        InitHp();
    }

    void Update()
    {
        if (IsDead)
            return;

        Vector3 targetPos;
        if (TargetUnit != null && !TargetUnit.IsDead)
            targetPos = TargetUnit.transform.position;
        else if (TargetStructure != null && !TargetStructure.IsDead)
            targetPos = TargetStructure.transform.position;
        else
            return;

        float dist = Vector2.Distance(transform.position, targetPos);
        if (dist > Config.AttackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, targetPos, Config.MoveSpeed * Time.deltaTime);
        }
        else
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                _attackTimer = Config.AttackCooldown;
                if (TargetUnit != null && !TargetUnit.IsDead)
                    TargetUnit.TakeDamage(Config.Damage);
                else if (TargetStructure != null && !TargetStructure.IsDead)
                    TargetStructure.TakeDamage(Config.Damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (TargetUnit != null && !TargetUnit.IsDead)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, TargetUnit.transform.position);
        }
        else if (TargetStructure != null && !TargetStructure.IsDead)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, TargetStructure.transform.position);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}
