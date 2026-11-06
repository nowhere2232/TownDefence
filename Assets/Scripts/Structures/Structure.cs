using UnityEngine;

public class Structure : Damageable
{
    public StructureConfig Config;
    public override float MaxHp => Config.MaxHp;

    [Tooltip("Optional fill transform")]
    public Transform HpBarFill;

    void Awake()
    {
        InitHp();
        UpdateHpBar();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        if (HpBarFill == null)
            return;
        float ratio = MaxHp > 0f ? CurrentHp / MaxHp : 0f;
        var s = HpBarFill.localScale;
        s.x = Mathf.Clamp01(ratio);
        HpBarFill.localScale = s;
    }
}
