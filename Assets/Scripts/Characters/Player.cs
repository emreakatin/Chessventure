using System;
using UnityEngine;
using ThirteenPixels.Soda;
public class Player : Character
{
    private const string PLAYER_LEVEL_KEY = "PlayerLevel";
    private int currentLevel = 0;
    public int killCount;
    private float lastAttackTime = 0f; // Son saldırı zamanı
    //private bool isAttacking = false;

     public GameEvent _onLevelUp;
     public GameEvent _onEnemyDied;

    protected override void Awake()
    {
        base.Awake();
        LoadPlayerLevel();
        InitializePlayer();
    }

    private void OnEnable()
    {
        _onEnemyDied.onRaise.AddListener(OnEnemyKilled);
    }
    
    private void OnDisable()
    {
        _onEnemyDied.onRaise.RemoveListener(OnEnemyKilled);
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
            
            Invoke("OnLevelUp",1);
            //OnLevelUp();
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
        _onLevelUp.Raise();
    }

    public override void OnDied()
    {
        base.OnDied();
        //_onPlayerDied();
    }

    protected override string GetPiecePrefabPath(ChessPieceType pieceType)
    {
        return string.Format(ResourceDirectories.PLAYER_PIECES_PATH, pieceType.ToString());
    }

    private void Update()
    {
        base.Update();
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

        // Saldırı girişi
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            TryAttack();
        }
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
            Debug.Log("Attacking");
            lastAttackTime = Time.time;
            isAttacking = true;
            animator.SetTrigger("Attack");

            Invoke("ResetAttack", characterData.attackCooldown);

            // Sadece düşman katmanını kontrol et
            int enemyLayerMask = LayerMask.GetMask("Enemy"); // "Enemy" adında bir katman oluşturmalısın
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, characterData.attackRange, enemyLayerMask);

            Collider closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var hitCollider in hitColliders)
            {
                
                IHealthSystem healthSystem = hitCollider.GetComponent<IHealthSystem>();
                if (healthSystem != null)
                {
                   
                    float distanceToEnemy = Vector3.Distance(transform.position, hitCollider.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hitCollider;
                    }
                }
            }

            if (closestEnemy != null)
            {
                Debug.Log("Hit: " + closestEnemy.name);
                IHealthSystem healthSystem = closestEnemy.GetComponent<IHealthSystem>();
        
                // Düşmana hasar ver
                if (!healthSystem.IsDead)
                {
                    float damage = characterData.attackPower;
                    healthSystem.TakeDamage(damage);
                }
               
            }
        }



    }

    
    private void ResetAttack()
    {
        isAttacking = false;
    }
} 