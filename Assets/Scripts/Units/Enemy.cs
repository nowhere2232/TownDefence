using UnityEngine;

public class Enemy : Damageable
{
    public EnemyConfig Config;
    public override float MaxHp => Config.MaxHp;

    public Unit TargetUnit { get; set; }
    public Structure TargetStructure { get; set; }

    private float _attackTimer;
    private UnitAnimator _animator;

    void Awake()
    {
        InitHp();
        _animator = GetComponent<UnitAnimator>();
    }

    void Start()
    {
        _animator?.PlaySpawn();
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
                Vector2 dir = (TargetUnit != null && !TargetUnit.IsDead)
                    ? (Vector2)(TargetUnit.transform.position - transform.position)
                    : (TargetStructure != null ? (Vector2)(TargetStructure.transform.position - transform.position) : Vector2.zero);
                if (TargetUnit != null && !TargetUnit.IsDead)
                    TargetUnit.TakeDamage(Config.Damage);
                else if (TargetStructure != null && !TargetStructure.IsDead)
                    TargetStructure.TakeDamage(Config.Damage);
                if (dir != Vector2.zero)
                    _animator?.PlayPunch(dir);
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

    protected override void Die()
    {
        if (_animator != null)
            _animator.PlayDeath(() => Destroy(gameObject));
        else
            Destroy(gameObject);
    }
}
