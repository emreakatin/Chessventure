using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [Header("Genel Ayarlar")]
    [SerializeField] protected bool destroyOnCollision = true;
    [SerializeField] protected float destroyDelay = 1f;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IHealthSystem playerHealth = other.GetComponent<IHealthSystem>();
            
            if (playerHealth != null && !playerHealth.IsDead)
            {
                ApplyDamage(playerHealth);
                
                if (destroyOnCollision)
                {
                    Destroy(gameObject, destroyDelay);
                }
            }
        }
    }

    protected abstract void ApplyDamage(IHealthSystem targetHealth);
} 