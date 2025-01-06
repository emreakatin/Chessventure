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

    protected bool isAttacking = false;

    protected ChessPieceType CurrentPieceType => characterData?.pieceType ?? ChessPieceType.Pawn;

    public CharacterData CharacterData => characterData; 
    protected virtual void Awake()
    {
        movementController = GetComponent<IMoveable>();
        healthSystem = GetComponent<IHealthSystem>();
        (healthSystem as HealthSystem)?.onDied.AddListener(OnDied);
        
        //(HealthSystem)healthSystem.onDied.AddListener(MyEventHandler);
      
    }
    
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

    public virtual void Update(){
        // Animasyon güncellemeleri
        UpdateAnimations();
    }

     private void UpdateAnimations()
    {

        if(animator != null)
        {

            // Örnek: Yürüyüş animasyonu
        if (movementController.IsMoving())
        {
            //Debug.Log("isMoving");
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            //Debug.Log("isNotMoving");
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

        // Koşma animasyonu
        if (movementController.IsRunning())
        {
            animator.SetBool("isRunning", true);
        }
        }
    }

    public virtual void OnDied(){
        animator.SetTrigger("Dead");
        Destroy(gameObject, 2);
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