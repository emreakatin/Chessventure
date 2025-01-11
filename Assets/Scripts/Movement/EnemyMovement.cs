using UnityEngine;
using UnityEngine.AI;
using ThirteenPixels.Soda;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : CharacterMovement
{
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private Vector3 patrolStartPoint;
    private Vector3 patrolDirection;
    
    [SerializeField] private float patrolDistance = 5f; // Nöbet mesafesi

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<Player>().transform;
        patrolStartPoint = transform.position;
        patrolDirection = Random.value > 0.5f ? Vector3.right : Vector3.left;
    }

    public void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolStartPoint + patrolDirection * patrolDistance) < 0.1f)
        {
            patrolDirection = -patrolDirection;
        }

        Vector3 targetPosition = patrolStartPoint + patrolDirection * patrolDistance;
        navMeshAgent.SetDestination(targetPosition);
        navMeshAgent.speed = currentMovementSpeed;
    }

    public void ChasePlayer()
    {
        navMeshAgent.SetDestination(playerTransform.position);
        navMeshAgent.speed = currentMovementSpeed * 1.5f;
    }

    public void StopMoving()
    {
        navMeshAgent.SetDestination(transform.position);
    }

    public override void Move(Vector3 direction)
    {
        navMeshAgent.SetDestination(transform.position + direction);
    }

    public override void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), 
                currentRotationSpeed * Time.deltaTime);
        }
    }

    public override bool IsMoving()
    {
        return navMeshAgent.velocity.magnitude > 0.1f;
    }

    public override bool IsRunning()
    {
        return navMeshAgent.velocity.magnitude > currentMovementSpeed;
    }
} 