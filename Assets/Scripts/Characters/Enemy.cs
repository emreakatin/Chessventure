using UnityEngine;
using ThirteenPixels.Soda;
using GameCore.Managers;
public class Enemy : Character
{
    [SerializeField] private GameEvent _onEnemyDied;
    [SerializeField] private GameEvent _onGameStateChanged;
    [SerializeField] private GameEvent _onLevelStart;
    private float lastAttackTime = 0f;
    private Player player;
    private EnemyMovement enemyMovement;
    private bool isGameStarted = false;

    private float detectionRange = 5f; // Oyuncuyu farketme mesafesi

    public GameEvent OnEnemyDied => _onEnemyDied;

    protected override void Awake()
    {
        base.Awake();
        enemyMovement = movementController as EnemyMovement;

        if(FindObjectOfType<GameManager>() == null)
        {
            isGameStarted = true;
        }
    }

    protected void Start()
    {
        player = FindObjectOfType<Player>();

        if(currentPiece == null)
        {
            InitializeEnemyType(player.CurrentPieceType);
              Debug.Log("Enemy Start");   
        }
      
        
    }

    private void OnEnable()
    {
        if (_onGameStateChanged != null)
            _onGameStateChanged.onRaise.AddListener(OnGameStateChanged);

        if (_onLevelStart != null)
            _onLevelStart.onRaise.AddListener(OnLevelStart);
    }

    private void OnDisable()
    {
        if (_onGameStateChanged != null)
            _onGameStateChanged.onRaise.RemoveListener(OnGameStateChanged);

        if (_onLevelStart != null)
            _onLevelStart.onRaise.RemoveListener(OnLevelStart);
    }

    private void OnGameStateChanged()
    {
        //isGameStarted = FindObjectOfType<GameManager>().GameStarted;
    }

    private void OnLevelStart()
    {
        isGameStarted = true;
    }

    private void Update()
    {
        base.Update();

        if (player == null || IsDead() || !isGameStarted) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        
        // Mesafeye göre davranış belirleme
        if (distanceToPlayer > detectionRange)
        {
            // Oyuncu uzaktaysa patrol yap
            enemyMovement.Patrol();
        }
        else if (distanceToPlayer > characterData.attackRange)
        {
            // Oyuncu detection range içindeyse ama attack range dışındaysa takip et
            enemyMovement.ChasePlayer();
            movementController.Rotate(directionToPlayer); // Oyuncuya dön
        }
        else
        {
            // Attack range içindeyse dur ve saldır
            enemyMovement.StopMoving();
            movementController.Rotate(directionToPlayer); // Oyuncuya dön

            if(!player.HealthSystem.IsDead){
                AttackPlayer();
            }
            
        }
    }

    private void AttackPlayer()
    {
        if (CanAttackPlayer())
        {
            TryAttack();
        }
    }

       
    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + characterData.attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");

            IHealthSystem healthSystem = player.GetComponent<IHealthSystem>();
            if (healthSystem != null && !healthSystem.IsDead)
            {
                float damage = characterData.attackPower;
                healthSystem.TakeDamage(damage);
                Debug.Log($"Enemy attacked player for {damage} damage!");
            }
        }
    }

    public void InitializeEnemyType(ChessPieceType playerPieceType)
    {
        // Player'ın seviyesine göre random bir düşman tipi seç
        ChessPieceType enemyType = GetRandomEnemyType(playerPieceType);
        Debug.Log("Enemy Initialize: " + enemyType);
        ChangePiece(enemyType);
    }

    private ChessPieceType GetRandomEnemyType(ChessPieceType playerPieceType)
    {
        // Player'ın seviyesinden düşük veya eşit bir seviye seç
        int maxLevel = (int)playerPieceType;
        //Debug.Log($"Player Piece Type: {playerPieceType} (Value: {maxLevel})");
        
        // maxLevel 0 ise en azından Pawn olsun
        if (maxLevel < 0) maxLevel = 0;
        
        int randomLevel = Random.Range(0, maxLevel + 1);
        //Debug.Log($"Random Enemy Level Selected: {randomLevel}");
        
        ChessPieceType selectedType = (ChessPieceType)randomLevel;
        //Debug.Log($"Selected Enemy Type: {selectedType}");
        
        return selectedType;
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
    
    public override void OnDied()
    {
        if (IsDead())
        {
            base.OnDied();
            animator.SetTrigger("Dead");
            Destroy(gameObject, 2);
            GetComponent<Collider>().enabled = false;
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

    private bool IsDead()
    {
        // Öldü mü kontrolü
        return healthSystem.CurrentHealth <= 0;
    }

} 