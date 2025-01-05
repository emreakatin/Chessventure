using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Type")]
    public ChessPieceType pieceType;
    public int requiredKillsToLevelUp = 3;

    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public int maxJumpCount = 2;
    
    [Header("Combat Settings")]
    public float maxHealth = 100f;
    public float attackPower = 10f;
    public float defense = 5f;
    public float attackRange = 2f;
    public float attackSpeed = 1f;
    public float attackCooldown = 1f;
    public float areaAttackRange = 5f;
    public float attackChance = 100f;
    
    [Header("Special Abilities")]
    public bool canUseShield;     // Kale için
    public bool canTeleport;      // At için
    public float specialAttackRange; // Fil, Vezir ve Şah için
} 