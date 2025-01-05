using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    private HealthSystem healthSystem;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDamageTaken.AddListener(PlayHitEffects);
        healthSystem.onDied.AddListener(PlayDeathEffects);
    }

    private void PlayHitEffects(float damage)
    {
        // Hit efektleri
    }

    private void PlayDeathEffects()
    {
        // Ölüm efektleri
    }
}