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
        // Player'ın seviyesine göre düşmanları spawnla
        ChessPieceType playerPieceType = player.CharacterData.pieceType;

        // Düşman türlerini kontrol et ve spawnla
        foreach (var enemyType in spawnSettings.enemyTypes)
        {
            if (enemyType.pieceType <= playerPieceType && Random.value < enemyType.spawnChance)
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

        // Get a random spawn area from the array
        Transform randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector3 spawnPosition = GetRandomPositionInArea(randomArea);

        // Havuzdan düşman al
        GameObject enemy = enemyPool.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPosition; // Pozisyonu ayarla
            enemy.SetActive(true); // Düşmanı aktif et
            enemy.transform.parent = transform;
            enemy.GetComponent<EnemyMovement>().NavMeshAgent.Warp(spawnPosition);

            // Düşman için pieceType'ı ayarla
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.ChangePiece(pieceType); // ChangePiece fonksiyonunu çağır
            currentEnemyCount++;
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

   private void SpawnEnemyNearPlayer()
{
    // Spawn alanları içerisinde karaktere en yakın olanı bul
    Transform nearestArea = FindNearestSpawnArea();
    if (nearestArea != null)
    {
        player = FindObjectOfType<Player>(); // Get the player reference again
        ChessPieceType playerPieceType = player.CharacterData.pieceType; // Get player's piece type

        // SpawnEnemyByPieceType method will spawn based on the player's piece type
        SpawnEnemyByPieceType(playerPieceType, nearestArea);
    }
}

private void SpawnEnemyByPieceType(ChessPieceType pieceType, Transform area)
{
    if (currentEnemyCount >= spawnSettings.maxEnemies) return;

    Vector3 spawnPosition = GetRandomPositionInArea(area); // Get a random position in the specified area

     // Havuzdan düşman al
        GameObject enemy = enemyPool.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = spawnPosition; // Pozisyonu ayarla
            enemy.SetActive(true); // Düşmanı aktif et
            enemy.transform.parent = transform;

            // Düşman için pieceType'ı ayarla
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            enemyComponent.ChangePiece(pieceType); // ChangePiece fonksiyonunu çağır
            currentEnemyCount++;
        }
}

    private Transform FindNearestSpawnArea()
    {
        Transform nearest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform area in spawnAreas)
        {
            float distance = Vector3.Distance(player.transform.position, area.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = area;
            }
        }
        return nearest;
    }
}