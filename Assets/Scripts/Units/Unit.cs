using TMPro;
using UnityEngine;

public class Unit : Damageable
{
    public UnitConfig Config;
    public override float MaxHp => Config.MaxHp;

    public float CurrentSatiety { get; private set; }

    public Enemy Target { get; set; }

    private float _attackTimer;
    private UnitAnimator _animator;

    void Awake()
    {
        InitHp();
        _animator = GetComponent<UnitAnimator>();
        CurrentSatiety = Config.ConsumesFood ? Config.MaxSatiety : 0f;
    }

    void Start()
    {
        _animator?.PlaySpawn();
    }

    void Update()
    {
        if (IsDead)
            return;
        if (Target == null || Target.IsDead)
            return;

        float dist = Vector2.Distance(transform.position, Target.transform.position);
        if (dist > Config.AttackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                Target.transform.position,
                Config.MoveSpeed * Time.deltaTime);
        }
        else
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0f)
            {
                _attackTimer = Config.AttackCooldown;
                Vector2 dir = Target.transform.position - transform.position;
                Target.TakeDamage(Config.Damage);
                _animator?.PlayPunch(dir);
            }
        }
    }

    public int TickConsumption(int availableWheat)
    {
        if (!Config.ConsumesFood || IsDead)
            return 0;

        int need = Mathf.CeilToInt(Config.FoodPerTick);
        if (availableWheat >= need)
        {
            CurrentSatiety = Mathf.Min(Config.MaxSatiety, CurrentSatiety + Config.SatietyPerMeal);
            return need;
        }

        CurrentSatiety = Mathf.Max(0f, CurrentSatiety - Config.SatietyDrainOnHunger);
        if (CurrentSatiety <= 0f)
            TakeDamage(MaxHp); // суицид
        return 0;
    }

    protected override void Die()
    {
        if (_animator != null)
            _animator.PlayDeath(() => Destroy(gameObject));
        else
            Destroy(gameObject);
    }
}
