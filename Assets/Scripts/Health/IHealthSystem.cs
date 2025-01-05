using UnityEngine;
using UnityEngine.Events;

public interface IHealthSystem
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsDead { get; }
    
    void TakeDamage(float damage);
    void Heal(float amount);
} 