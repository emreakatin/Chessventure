using UnityEngine;
using ThirteenPixels.Soda;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnSettings spawnSettings; // Spawn ayarları
    [SerializeField] private GameEvent _onLevelLoad;
    [SerializeField] private GameEvent _onGameLevelStartRequest;
    [SerializeField] private GameEvent _onEnemyDied;
    [SerializeField] private Player player; // Player referansı
    private int currentEnemyCount = 0; // Şu anki düşman sayısı
    private EnemyPool enemyPool; // Düşman havuzu

    [SerializeField] private Transform[] spawnAreas; // Spawn alanları

    private void Start()
    {
        enemyPool = GetComponent<EnemyPool>(); // EnemyPool bileşenini al
        // SpawnEnemies(); // Uncomment this if you want to spawn enemies immediately when starting
    }

    private void OnEnable()
    {
        _onGameLevelStartRequest.onRaise.AddListener(SpawnEnemies);
        _onEnemyDied.onRaise.AddListener(OnEnemyDied);
    }

    private void OnDisable()
    {
        _onGameLevelStartRequest.onRaise.RemoveListener(SpawnEnemies);
        _onEnemyDied.onRaise.RemoveListener(OnEnemyDied);
    }

    public void SpawnEnemies()
    {
        player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        ChessPieceType playerPieceType = player.CharacterData.pieceType;
        Debug.Log($"Spawning enemies for player type: {playerPieceType}");

        // Düşman türlerini kontrol et ve spawnla
        foreach (var enemyType in spawnSettings.enemyTypes)
        {
            if ((int)enemyType.pieceType <= (int)playerPieceType && Random.value < enemyType.spawnChance)
            {
                for (int i = 0; i < 20; i++)
                {
                    SpawnEnemyInRandomArea(enemyType.pieceType);
                }
            }
        }
    }

    private void SpawnEnemyInRandomArea(ChessPieceType pieceType)
    {
        if (currentEnemyCount >= spawnSettings.maxEnemies) return;

        Transform randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector3 spawnPosition = GetRandomPositionInArea(randomArea);

        GameObject enemy = enemyPool.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
            enemy.transform.parent = transform;
            enemy.GetComponent<EnemyMovement>().NavMeshAgent.Warp(spawnPosition);


            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.ChangePiece(pieceType);
            currentEnemyCount++;
            
            //Debug.Log($"Spawned enemy of type: {enemyComponent.CharacterData.pieceType}");
        }
    }

    private Vector3 GetRandomPositionInArea(Transform area)
    {
        // Spawn alanı içindeki rastgele bir pozisyon hesapla
        Vector3 randomPoint = new Vector3(
            Random.Range(area.position.x - area.localScale.x / 2, area.position.x + area.localScale.x / 2),
            area.position.y, 
            Random.Range(area.position.z - area.localScale.z / 2, area.position.z + area.localScale.z / 2)
        );
        return randomPoint;
    }

    public void OnEnemyDied()
    {
        //currentEnemyCount--; // Düşman öldüğünde sayıyı azalt
        //pawnEnemyNearPlayer(); // Yeni düşman spawnla
    }
}