using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy), typeof(NavMeshAgent))]
public class EnemyMovement : CharacterMovement
{
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void Move(Vector3 direction)
    {
        // Düşmanın hareket etme mantığı
        navMeshAgent.SetDestination(direction);
        
        // Eğer düşman oyuncuya yakınsa koşma mantığını kontrol et
        if (Vector3.Distance(transform.position, direction) < 5f) // Örnek mesafe
        {
            navMeshAgent.speed = currentMovementSpeed * 1.5f; // Koşma hızı
        }
        else
        {
            navMeshAgent.speed = currentMovementSpeed; // Normal hız
        }
    }

    public override void Rotate(Vector3 direction)
    {
        // Düşmanın yönlendirme mantığı
        if (direction.magnitude > 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, currentRotationSpeed * Time.deltaTime);
        }
    }

    public override bool IsMoving()
    {
        // Düşmanın hareket edip etmediğini kontrol et
        return navMeshAgent.velocity.magnitude > 0;
    }

    public override bool IsRunning()
    {
        // Düşmanın koşma durumu için kendi mantığınızı ekleyebilirsiniz
        return navMeshAgent.speed > currentMovementSpeed; // Eğer hız normal hızdan fazlaysa koşuyor demektir
    }
} 