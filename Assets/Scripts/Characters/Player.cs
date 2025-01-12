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
     public GameEvent _onPlayerDied;

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
        Debug.Log($"Loaded player level: {currentLevel}");
    }

    private void SavePlayerLevel()
    {
        PlayerPrefs.SetInt(PLAYER_LEVEL_KEY, currentLevel);
        PlayerPrefs.Save();
    }

    private void InitializePlayer()
    {
        ChessPieceType savedPieceType = (ChessPieceType)currentLevel;
        Debug.Log($"Initializing player with piece type: {savedPieceType}");
        
        if (!System.Enum.IsDefined(typeof(ChessPieceType), savedPieceType))
        {
            currentLevel = 0;
            savedPieceType = ChessPieceType.Pawn;
            Debug.Log("Invalid piece type, resetting to Pawn");
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
            
            Invoke("OnLevelUp",1.5f);
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
        animator.SetTrigger("Dead");
        //Destroy(gameObject, 2);
        _onPlayerDied.Raise();
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
        if (Input.GetButtonDown("Fire1") && !isAttacking && !healthSystem.IsDead)
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

    private IHealthSystem targetHealthSystem;

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
            int enemyLayerMask = LayerMask.GetMask("Enemy");
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, characterData.attackRange, enemyLayerMask);

            Collider closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (var hitCollider in hitColliders)
            {
                IHealthSystem opponentHealthSystem = hitCollider.GetComponent<IHealthSystem>();
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
                targetHealthSystem = closestEnemy.GetComponent<IHealthSystem>();
    
                // Düşmana hasar ver
                if (!targetHealthSystem.IsDead)
                {
                    Invoke("GiveDelayedDamage", 0.5f);
                }
            }
        }
    }

    private void GiveDelayedDamage()
    {
        if (targetHealthSystem != null && !targetHealthSystem.IsDead)
        {
            targetHealthSystem.TakeDamage(characterData.attackPower);
        }
    }
    private void ResetAttack()
    {
        isAttacking = false;
    }
} 