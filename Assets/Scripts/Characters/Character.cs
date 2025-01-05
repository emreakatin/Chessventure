using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected Transform pieceHolder;
    protected CharacterData characterData;
    
    protected IMoveable movementController;
    protected IHealthSystem healthSystem;
    protected GameObject currentPieceInstance;
    protected ChessPiece currentPiece;
    [SerializeField] protected Animator animator;

    protected ChessPieceType CurrentPieceType => characterData?.pieceType ?? ChessPieceType.Pawn;

    protected virtual void Awake()
    {
        movementController = GetComponent<IMoveable>();
        healthSystem = GetComponent<IHealthSystem>();
    }

    public CharacterData CharacterData => characterData;

    protected abstract string GetPiecePrefabPath(ChessPieceType pieceType);

    public virtual void ChangePiece(ChessPieceType pieceType)
    {
        if (currentPieceInstance != null)
        {
            Destroy(currentPieceInstance);
        }

        string prefabPath = GetPiecePrefabPath(pieceType);
        GameObject piecePrefab = Resources.Load<GameObject>(prefabPath);

        if (piecePrefab != null)
        {
            // Spawn new piece
            currentPieceInstance = Instantiate(piecePrefab, pieceHolder);
            
            // Pozisyonu ayarla
            ChessPiece chessPiece = currentPieceInstance.GetComponent<ChessPiece>();
            currentPieceInstance.transform.localPosition = chessPiece.SpawnOffset;
            currentPieceInstance.transform.localRotation = Quaternion.identity;

            // Get references
            currentPiece = chessPiece;
            animator = currentPiece.Animator;

            // Update character data
            characterData = currentPiece.Data;

            UpdateSystems();
            OnPieceChanged();
        }
        else
        {
            Debug.LogError($"Chess piece prefab not found at path: {prefabPath}");
        }
    }

    protected virtual void UpdateSystems()
    {
        // Update movement system
        if (movementController is CharacterMovement characterMovement)
        {
            characterMovement.UpdateMovementData(characterData);
        }

        // Update health system
        if (healthSystem is HealthSystem health)
        {
            health.UpdateHealthData(characterData);
        }
    }

    protected virtual void OnPieceChanged()
    {
        // Override in derived classes for specific behavior
    }
} 