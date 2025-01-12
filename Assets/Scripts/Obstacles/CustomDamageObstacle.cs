using UnityEngine;

public class CustomDamageObstacle : Obstacle
{
    [Header("Hasar Ayarları")]
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private bool usePercentageDamage = false;
    [SerializeField] [Range(0f, 1f)] private float damagePercentage = 0.2f;

    protected override void ApplyDamage(IHealthSystem targetHealth)
    {
        float damage;
        
        if (usePercentageDamage)
        {
            // Yüzdelik hasar
            damage = targetHealth.CurrentHealth * damagePercentage;
        }
        else
        {
            // Sabit hasar
            damage = damageAmount;
        }
        
        targetHealth.TakeDamage(damage);
        
      
    }
} 