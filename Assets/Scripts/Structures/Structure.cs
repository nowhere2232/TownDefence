using UnityEngine;

public class Structure : Damageable
{
    public StructureConfig Config;
    public override float MaxHp => Config.MaxHp;

    [Tooltip("Optional fill transform")]
    public Transform HpBarFill;

    private float _hpBarFullWidth;
    private Vector3 _hpBarFullCenter;

    void Awake()
    {
        InitHp();
        if (HpBarFill != null )
        {
            _hpBarFullWidth = HpBarFill.localScale.x;
            _hpBarFullCenter = HpBarFill.localPosition;
        }
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

        var p = _hpBarFullCenter;
        p.x -= (_hpBarFullWidth - s.x) * 0.5f;
        HpBarFill.localPosition = p;
    }
}
