using UnityEngine;
using ThirteenPixels.Soda;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnSettings spawnSettings; // Spawn ayarları
    [SerializeField] private GameEvent _onGameLevelStartRequest;
    [SerializeField] private GameEvent _onEnemyDied;
    [SerializeField] private Player player; // Player referansı
    private int currentEnemyCount = 0; // Şu anki düşman sayısı
    private EnemyPool enemyPool; // Düşman havuzu

    private void Start()
    {
        enemyPool = GetComponent<EnemyPool>(); // EnemyPool bileşenini al
        //SpawnEnemies();
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
                for(int i = 0; i < 10; i++)
                {
                    SpawnEnemy(enemyType.pieceType);
                }
            }
        }
    }

    private void SpawnEnemy(ChessPieceType pieceType)
    {
        if (currentEnemyCount >= spawnSettings.maxEnemies) return;

        // Spawnlama mesafesini ayarla
        Vector3 spawnPosition = GetRandomSpawnPosition();

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

    private Vector3 GetRandomSpawnPosition()
    {
        // Player'dan belirli bir mesafede spawnlama
        Vector3 randomDirection = Random.insideUnitSphere * spawnSettings.spawnDistance;
        randomDirection.y = 0; // Y eksenini sıfırla
        return player.transform.position + randomDirection;
    }

    public void OnEnemyDied()
    {
        currentEnemyCount--; // Düşman öldüğünde sayıyı azalt
        SpawnEnemies(); // Yeni düşman spawnla
    }
} 