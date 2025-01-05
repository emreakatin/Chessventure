using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 spawnOffset;
    
    public CharacterData Data => characterData;
    public Animator Animator => animator;
    public Vector3 SpawnOffset => spawnOffset;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
} 