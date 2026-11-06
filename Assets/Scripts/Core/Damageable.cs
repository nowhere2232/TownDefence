using System;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public abstract float MaxHp { get; }
    public float CurrentHp { get; protected set; }
    public bool IsDead => CurrentHp <= 0f;

    public event Action<Damageable> OnDied;

    protected void InitHp()
    {
        CurrentHp = MaxHp;
    }

    public virtual void TakeDamage(float amount)
    {
        if (IsDead)
            return;
        CurrentHp = Mathf.Max(0f, CurrentHp - amount);
        if (CurrentHp <= 0)
            OnDied?.Invoke(this);
    }

    public void ResetHp()
    {
        CurrentHp = MaxHp;
    }
}
