using UnityEngine;

public class Unit : Damageable
{
    public UnitConfig Config;
    public override float MaxHp => Config.MaxHp;

    public float CurrentSatiety { get; private set; }

    public Enemy Target { get; set; }

    private float _attackTimer;

    void Awake()
    {
        InitHp();
        CurrentSatiety = Config.ConsumesFood ? Config.MaxSatiety : 0f;
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
                Target.TakeDamage(Config.Damage);
            }
        }
    }

    public int TickConsumption(int availableWheat)
    {
        if (!Config.ConsumesFood || IsDead)
            return 0;

        int need = Mathf.CeilToInt(Config.FoodPerTick);
        if (CurrentSatiety < Config.MaxSatiety && availableWheat >= need)
        {
            CurrentSatiety = Mathf.Min(Config.MaxSatiety, CurrentSatiety + Config.SatietyPerMeal);
            return need;
        }

        CurrentSatiety = Mathf.Max(0f, CurrentSatiety - Config.SatietyDrainOnHunger);
        if (CurrentSatiety <= 0f)
            TakeDamage(MaxHp); // суицид
        return 0;
    }
}
