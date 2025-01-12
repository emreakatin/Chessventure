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
                for (int i = 0; i < 30; i++)
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
        // Spawn alanının boyutlarını al
        float areaWidth = area.localScale.x;
        float areaLength = area.localScale.z;
        
        // Kenarlardan birini rastgele seç (0: Sol, 1: Sağ, 2: Üst, 3: Alt)
        int edge = Random.Range(0, 4);
        Vector3 randomPoint;

        switch (edge)
        {
            case 0: // Sol kenar
                randomPoint = new Vector3(
                    area.position.x - areaWidth/2,
                    area.position.y,
                    Random.Range(area.position.z - areaLength/2, area.position.z + areaLength/2)
                );
                break;
            case 1: // Sağ kenar
                randomPoint = new Vector3(
                    area.position.x + areaWidth/2,
                    area.position.y,
                    Random.Range(area.position.z - areaLength/2, area.position.z + areaLength/2)
                );
                break;
            case 2: // Üst kenar
                randomPoint = new Vector3(
                    Random.Range(area.position.x - areaWidth/2, area.position.x + areaWidth/2),
                    area.position.y,
                    area.position.z + areaLength/2
                );
                break;
            default: // Alt kenar
                randomPoint = new Vector3(
                    Random.Range(area.position.x - areaWidth/2, area.position.x + areaWidth/2),
                    area.position.y,
                    area.position.z - areaLength/2
                );
                break;
        }

        return randomPoint;
    }

    public void OnEnemyDied()
    {

          player = FindObjectOfType<Player>();
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        ChessPieceType playerPieceType = player.CharacterData.pieceType;
        Debug.Log($"Spawning enemies for player type: {playerPieceType}");
        //currentEnemyCount--; // Düşman öldüğünde sayıyı azalt
        //pawnEnemyNearPlayer(); // Yeni düşman spawnla
         foreach (var enemyType in spawnSettings.enemyTypes)
        {
            if ((int)enemyType.pieceType <= (int)playerPieceType && Random.value < enemyType.spawnChance)
            {
                for (int i = 0; i < 2; i++)
                {
                    SpawnEnemyInRandomArea(enemyType.pieceType);
                }
            }
        }
    }

    private void CheckLevelComplete()
    {
      /*   if (activeEnemies.Count == 0 && totalSpawnedEnemies >= maxEnemies)
        {
            Debug.Log("Level Complete!");
            SoundManager.Instance.PlayLevelCompleteSound();
            _onLevelComplete.Raise();
        } */
    }
}