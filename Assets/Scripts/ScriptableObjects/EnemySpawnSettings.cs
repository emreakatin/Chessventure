using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Spawn Settings", menuName = "Game/Enemy Spawn Settings")]
public class EnemySpawnSettings : ScriptableObject
{
    public int maxEnemies; // Maksimum düşman sayısı
    public float spawnDistance; // Spawnlama mesafesi
    
    // Düşman türleri ve olasılıkları
    public EnemySpawnType[] enemyTypes; // Düşman türleri
}

[System.Serializable]
public class EnemySpawnType
{
    public ChessPieceType pieceType; // Düşman tipi
    public float spawnChance; // Düşman spawn olasılığı
} 