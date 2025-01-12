using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [Header("Particle Sistemleri")]
    [SerializeField] private ParticleSystem levelUpParticle;
    [SerializeField] private ParticleSystem playerAttackParticle;
    [SerializeField] private ParticleSystem enemyAttackParticle;
    [SerializeField] private ParticleSystem playerDeathParticle;

    [SerializeField] private ParticleSystem healParticle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLevelUpEffect(Vector3 position)
    {
        if (levelUpParticle != null)
        {
            ParticleSystem particle = Instantiate(levelUpParticle, position, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }
    }

    public void PlayPlayerAttackEffect(Vector3 position, Vector3 direction)
    {
        if (playerAttackParticle != null)
        {
            ParticleSystem particle = Instantiate(playerAttackParticle, position, Quaternion.LookRotation(direction));
            Destroy(particle.gameObject, particle.main.duration);
        }
    }

    public void PlayEnemyAttackEffect(Vector3 position, Vector3 direction)
    {
        if (enemyAttackParticle != null)
        {
            ParticleSystem particle = Instantiate(enemyAttackParticle, position, Quaternion.LookRotation(direction));
            Destroy(particle.gameObject, particle.main.duration);
        }
    }

    public void PlayPlayerDeathEffect(Vector3 position)
    {
        if (playerDeathParticle != null)
        {
            ParticleSystem particle = Instantiate(playerDeathParticle, position, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }
    }

    public void PlayHealEffect(Vector3 position)
    {
        if (healParticle != null)
        {
            ParticleSystem particle = Instantiate(healParticle, position, Quaternion.identity);
            Destroy(particle.gameObject, particle.main.duration);
        }
    }
} 