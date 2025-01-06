using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Character))]
public class HealthSystem : MonoBehaviour, IHealthSystem
{
    private float maxHealth;
    public float currentHealth;
    private float defense;
    
    public UnityEvent<float> onHealthChanged;
    public UnityEvent<float> onDamageTaken;
    public UnityEvent<float> onHealed;
    public UnityEvent onDied;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    

    public void TakeDamage(float damage)
    {
        //if (IsDead) return;

        float actualDamage = CalculateDamage(damage);
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);

        onDamageTaken?.Invoke(actualDamage);
        onHealthChanged?.Invoke(currentHealth);

        if (IsDead)
        {
            onDied?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        float actualHeal = Mathf.Min(MaxHealth - currentHealth, amount);
        currentHealth = Mathf.Min(MaxHealth, currentHealth + actualHeal);

        onHealed?.Invoke(actualHeal);
        onHealthChanged?.Invoke(currentHealth);
    }

    private float CalculateDamage(float incomingDamage)
    {
        return Mathf.Max(0, incomingDamage - defense);
    }

    public void UpdateHealthData(CharacterData data)
    {
        maxHealth = data.maxHealth;
        defense = data.defense;
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }
} 