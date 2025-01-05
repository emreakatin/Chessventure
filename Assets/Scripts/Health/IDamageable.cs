using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    float CurrentHealth { get; }
    bool IsDead { get; }
} 