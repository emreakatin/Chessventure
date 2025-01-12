using UnityEngine;
using UnityEngine.AI;
using ThirteenPixels.Soda;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : CharacterMovement
{
    private NavMeshAgent navMeshAgent;

    private Transform playerTransform;

    private Vector3 patrolStartPoint;

    private Vector3 currentPatrolTarget;

    private bool isWaitingAtPatrolPoint = false;

    private float waitStartTime;

    private bool isMovingRight = true; // Sağa mı sola mı hareket ediyor

    private float patrolDistance = 5f; // Nöbet mesafesi
    private float waitTimeAtPatrolPoint = 1f; // Her noktada bekleme süresi
    //private float minPatrolDistance = 6f; // Minimum devriye mesafesi
    //private float animationSpeedThreshold = 0.1f; // Animasyon geçiş eşiği






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
        isMovingRight = Random.value > 0.5f;
        SetNewPatrolTarget();
    }

    private void SetNewPatrolTarget()
    {
        // Sağa veya sola hareket yönünü belirle
        patrolDistance = Random.Range(3f, 7f);
        waitTimeAtPatrolPoint = Random.Range(1f, 2f);
        
        Vector3 direction = isMovingRight ? Vector3.right : Vector3.left;
        currentPatrolTarget = patrolStartPoint + direction * patrolDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(currentPatrolTarget, out hit, patrolDistance, NavMesh.AllAreas))
        {
            currentPatrolTarget = hit.position;
        }
        else
        {
            Debug.LogError("NavMesh.SamplePosition failed to find a valid position");
        }
    }

    public void Patrol()
    {
        if (isWaitingAtPatrolPoint)
        {
            if (Time.time - waitStartTime >= waitTimeAtPatrolPoint)
            {
                isWaitingAtPatrolPoint = false;
                isMovingRight = !isMovingRight; // Yönü değiştir
                SetNewPatrolTarget();
            }
            navMeshAgent.isStopped = true;
            return;
        }

        navMeshAgent.isStopped = false;
        if (Vector3.Distance(transform.position, currentPatrolTarget) < 2f)
        {
            isWaitingAtPatrolPoint = true;
            waitStartTime = Time.time;
            return;
        }

        navMeshAgent.SetDestination(currentPatrolTarget);
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
        return navMeshAgent.velocity.magnitude > 0.1f && navMeshAgent.remainingDistance > 0.1f;
    }

    public override bool IsRunning()
    {
        return navMeshAgent.velocity.magnitude > currentMovementSpeed * 0.8f && navMeshAgent.remainingDistance > 0.1f;
    }
} 