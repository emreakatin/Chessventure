using UnityEngine;
using UnityEngine.AI;
using ThirteenPixels.Soda;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : CharacterMovement
{
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private Vector3 patrolStartPoint; // Nöbet başlangıç noktası
    private Vector3 patrolDirection; // Nöbet yönü
    private float patrolDistance = 5f; // Nöbet mesafesi
    private bool isPatrolling = false; // Nöbet durumu

    public bool IsPatrolling => isPatrolling;

    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        
        playerTransform = FindObjectOfType<Player>().transform; // Oyuncu referansını al
     

        // Nöbet başlangıç noktasını belirle
        patrolStartPoint = transform.position;
        // Sağ veya sol yön belirle
        patrolDirection = Random.value > 0.5f ? Vector3.right : Vector3.left; // Sağ veya sol
        isPatrolling = true; // Nöbeti başlat
    }

    private void Update()
    {
      
    }

     public void UpdateMovementData(CharacterData data)
    {
        base.UpdateMovementData(data);
        navMeshAgent.speed = currentMovementSpeed;
    }

    public void Patrol()
    {
        // Nöbet mesafesi kadar hareket et
        if (Vector3.Distance(transform.position, patrolStartPoint + patrolDirection * patrolDistance) < 0.1f)
        {
            // Başlangıç noktasına geri dön
            patrolDirection = -patrolDirection; // Yönü değiştir
        }

        navMeshAgent.SetDestination(patrolStartPoint + patrolDirection * patrolDistance);
    }

    public void ChasePlayer()
    {
        navMeshAgent.SetDestination(playerTransform.position);
    }

    public override void Move(Vector3 direction)
    {
        navMeshAgent.SetDestination(direction);
    }

    public override void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), currentRotationSpeed * Time.deltaTime);
    }


    public override bool IsMoving()
    {
        return navMeshAgent.velocity.magnitude > 0.1f;

    }

    public override bool IsRunning()
    {
        return navMeshAgent.speed > currentMovementSpeed;
    }

   
} 