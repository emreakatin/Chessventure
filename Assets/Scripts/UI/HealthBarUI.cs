using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Player player; // Player referansı
    [SerializeField] private Slider healthSlider; // UI'daki sağlık çubuğu
    [SerializeField] private float smoothSpeed = 0.1f; // Yumuşak geçiş hızı

    private float targetHealth; // Hedef sağlık değeri

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>(); // Player'ı bul
        }

        // Sağlık çubuğunu başlat
        UpdateHealthBar();
    }

    private void Update()
    {
        UpdateHealthBar(); // Her frame'de sağlık çubuğunu güncelle
    }

    private void UpdateHealthBar()
    {
        if (player != null && player.CharacterData != null)
        {
            float currentHealth = player.HealthSystem.CurrentHealth;
            float maxHealth = player.CharacterData.maxHealth;

            targetHealth = currentHealth / maxHealth; // Hedef sağlık değerini ayarla

            // Yumuşak geçiş
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetHealth, smoothSpeed);
        }
    }
} 