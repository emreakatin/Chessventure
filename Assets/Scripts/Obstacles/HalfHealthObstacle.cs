using UnityEngine;

public class HalfHealthObstacle : Obstacle
{
    protected override void ApplyDamage(IHealthSystem targetHealth)
    {
        float currentHealth = targetHealth.CurrentHealth;
        float damage = currentHealth * 0.5f; // Mevcut canın yarısı kadar hasar
        
        targetHealth.TakeDamage(damage);
        
       
    }
} 