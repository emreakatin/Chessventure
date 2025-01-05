using UnityEngine;

public class Player : Character
{
    private const string PLAYER_LEVEL_KEY = "PlayerLevel";
    private int currentLevel = 0;
    protected int killCount;
    private float lastAttackTime = 0f; // Son saldırı zamanı

    protected override void Awake()
    {
        base.Awake();
        LoadPlayerLevel();
        InitializePlayer();
    }

    private void LoadPlayerLevel()
    {
        currentLevel = PlayerPrefs.GetInt(PLAYER_LEVEL_KEY, 0);
    }

    private void SavePlayerLevel()
    {
        PlayerPrefs.SetInt(PLAYER_LEVEL_KEY, currentLevel);
        PlayerPrefs.Save();
    }

    private void InitializePlayer()
    {
        // Kaydedilen levele göre başlat
        ChessPieceType savedPieceType = (ChessPieceType)currentLevel;
        
        // Geçerli bir piece type mı kontrol et
        if (!System.Enum.IsDefined(typeof(ChessPieceType), savedPieceType))
        {
            currentLevel = 0;
            savedPieceType = ChessPieceType.Pawn;
        }

        ChangePiece(savedPieceType);
    }

    protected virtual void OnEnemyKilled()
    {
        killCount++;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (killCount >= characterData.requiredKillsToLevelUp)
        {
            LevelUp();  
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        ChessPieceType nextPieceType = (ChessPieceType)currentLevel;
        
        if (System.Enum.IsDefined(typeof(ChessPieceType), nextPieceType))
        {
            ChangePiece(nextPieceType);
            killCount = 0;
            SavePlayerLevel(); // Level atladığında kaydet
            OnLevelUp();
        }
    }

    protected override void OnPieceChanged()
    {
        UpdateCharacterAbilities();
    }

    private void UpdateCharacterAbilities()
    {
        switch (CurrentPieceType)
        {
            case ChessPieceType.Pawn:
                // Piyon yeteneklerini aktifleştir
                break;
            case ChessPieceType.Knight:
                // At yeteneklerini aktifleştir
                break;
            // ... diğer piece'ler
        }
    }

    private void OnLevelUp()
    {
        Debug.Log($"Leveled up to {CurrentPieceType}!");
    }

    protected override string GetPiecePrefabPath(ChessPieceType pieceType)
    {
        return string.Format(ResourceDirectories.PLAYER_PIECES_PATH, pieceType.ToString());
    }

    private void Update()
    {
        // Test için level atlama
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test: Level up triggered!");
            LevelUp();
        }

        // Test için progress reset
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Test: Progress reset!");
            ResetPlayerProgress();
        }

        // Animasyon güncellemeleri
        UpdateAnimations();

        // Saldırı kontrolü
        if (Input.GetMouseButtonDown(0)) // Sol tık
        {
            TryAttack();
        }
    }

    private void UpdateAnimations()
    {

        if(animator != null)
        {
            // Örnek: Yürüyüş animasyonu
        if (movementController.IsMoving())
        {
            //Debug.Log("isMoving");
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            //Debug.Log("isNotMoving");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Koşma animasyonu
        if (movementController.IsRunning())
        {
            animator.SetBool("isRunning", true);
        }

        // Ölme animasyonu
        if (healthSystem.IsDead)
        {
            animator.SetBool("isDead", true);
        }
        else
        {
            animator.SetBool("isDead", false);
        }

        // Saldırı animasyonu
        if (Input.GetKeyDown(KeyCode.Space)) // Örnek: Boşluk tuşu ile saldırı
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }}
    }

    public void ResetPlayerProgress()
    {
        currentLevel = 0;
        killCount = 0;
        SavePlayerLevel();
        InitializePlayer();
    }

    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + characterData.attackCooldown)
        {
            lastAttackTime = Time.time;

            // Karakterin baktığı yönde düşman var mı kontrol et
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, characterData.attackRange))
            {
                IHealthSystem healthSystem = hit.collider.GetComponent<IHealthSystem>();
                if (healthSystem != null)
                {
                    // Düşmana hasar ver
                    float damage = characterData.attackPower - healthSystem.CurrentHealth; // Düşmanın savunmasını çıkar
                    damage = Mathf.Max(damage, 0); // Negatif hasar olmasın
                    healthSystem.TakeDamage(damage);
                    Debug.Log($"Attacked {hit.collider.name} for {damage} damage!");
                }
            }
        }
    }
} 