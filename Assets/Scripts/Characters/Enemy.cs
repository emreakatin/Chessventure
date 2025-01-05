using UnityEngine;
using ThirteenPixels.Soda;

public class Enemy : Character
{
    [SerializeField] private GameEvent _onEnemyDied;
    private float lastAttackTime = 0f; // Son saldırı zamanı

    protected override void Awake()
    {
        base.Awake();
        //healthSystem.onDied.AddListener(OnEnemyDied);
    }

    protected override void OnPieceChanged()
    {
        InitializeEnemyAbilities();
    }

    private void InitializeEnemyAbilities()
    {
        switch (CurrentPieceType)
        {
            case ChessPieceType.Pawn:
                // Piyon davranışları
                break;
            case ChessPieceType.Knight:
                // At davranışları
                break;
            // ... diğer tipler
        }
    }


    protected override string GetPiecePrefabPath(ChessPieceType pieceType)
    {
        return string.Format(ResourceDirectories.ENEMY_PIECES_PATH, pieceType.ToString());
    }

    private void OnEnemyDied()
    {
        _onEnemyDied.Raise();
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        // Düşmanın saldırı mantığı
        if (CanAttackPlayer())
        {
            TryAttack();
        }
    }

    private bool CanAttackPlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= characterData.attackRange)
        {
            return Random.value < characterData.attackChance / 100f; // Saldırı yapma olasılığı
        }
        return false;
    }

    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + characterData.attackCooldown)
        {
            lastAttackTime = Time.time;

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                IHealthSystem healthSystem = player.GetComponent<IHealthSystem>();
                if (healthSystem != null)
                {
                    float damage = characterData.attackPower - player.CharacterData.defense; // Düşmanın savunmasını çıkar
                    damage = Mathf.Max(damage, 0); // Negatif hasar olmasın
                    healthSystem.TakeDamage(damage);
                    Debug.Log($"Enemy attacked {player.name} for {damage} damage!");
                }
            }
        }
    }
} 