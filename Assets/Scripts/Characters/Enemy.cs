using UnityEngine;
using ThirteenPixels.Soda;

public class Enemy : Character
{
    [SerializeField] private GameEvent _onEnemyDied;
    private float lastAttackTime = 0f; // Son saldırı zamanı
    private Player player;
    protected override void Awake()
    {
        base.Awake();
        //healthSystem.onDied.AddListener(OnEnemyDied);
    }

    protected void Start()
    {
        //(enemyMovement as EnemyMovement).OnEnemyAttack.AddListener(AttackPlayer);
        player = FindObjectOfType<Player>();
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
    
    private void Update()
    {
        base.Update();

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer > characterData.attackRange)
        {
            if ((movementController as EnemyMovement).IsPatrolling)
            {
                //(movementController as EnemyMovement).Patrol(); // Nöbet tutma hareketi
            }
        }
        else if (distanceToPlayer <= characterData.attackRange && distanceToPlayer > characterData.attackRange / 2)
        {
           (movementController as EnemyMovement).ChasePlayer(); // Oyuncuya koşma
        }
        else
        {
            //_onEnemyAttack.Raise();
            AttackPlayer(); // Oyuncuya saldırma
        }
    }
    private void AttackPlayer()
    {
        if (CanAttackPlayer())
        {
            // Saldırı yapma kodu
            //TryAttack(); // Düşmanın saldırı fonksiyonu
        }
    }

    public override void OnDied()
    {
        if (IsDead())
        {
            base.OnDied();
            _onEnemyDied.Raise();
        }
      
    }
    public bool CanAttackPlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= characterData.attackRange)
        {
            // Saldırı yapma koşulları
            if (Time.time >= lastAttackTime + characterData.attackCooldown && !IsDead())
            {
                return true; // Saldırı yapabilir
            }
        }
        return false; // Saldırı yapamaz
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
                    float damage = characterData.attackPower;
                    healthSystem.TakeDamage(damage);
                }
            }
        }
    }

      private bool IsDead()
    {
        // Öldü mü kontrolü
        return healthSystem.CurrentHealth <= 0;
    }

} 