using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float smoothSpeed = 5f;
    //[SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Transform mainCamera;
    private float targetHealth;
   

    
    private void Awake()
    {
        
        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
        }
    }

    private void OnEnable()
    {
        //enemy.OnEnemyDied.onRaise.AddListener(OnEnemyDied);
    }   

    private void OnDisable()
    {
        //enemy.OnEnemyDied.onRaise.RemoveListener(OnEnemyDied);
    }

    private void Start()
    {
        if (enemy != null)
        {
            mainCamera = Camera.main.transform;
            UpdateHealthBar();
        }
    }

    private void Update()
    {
        if (enemy != null)
        {
            // Health bar'ı güncelle
            UpdateHealthBar();

            // Yumuşak geçiş ile health bar'ı güncelle
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealth, Time.deltaTime * smoothSpeed);
             //gameObject.SetActive(!enemy.HealthSystem.IsDead);
            
            // Düşman ölü ise health bar'ı gizle, değilse göster
        
        }
    }

    private void OnEnemyDied()
    {
        Invoke("DisableHealthBar", 1f);
    }

    private void DisableHealthBar()
    {
        healthSlider.fillRect.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }

    private void UpdateHealthBar()
    {
        if (enemy != null && enemy.CharacterData != null)
        {
            float currentHealth = enemy.HealthSystem.CurrentHealth;
            float maxHealth = enemy.CharacterData.maxHealth;
            targetHealth = currentHealth / maxHealth;
        }
    }
} 