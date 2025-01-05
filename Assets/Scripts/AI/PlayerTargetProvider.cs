using UnityEngine;

public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
{
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool HasTarget => target != null;

    public Vector3 GetTargetPosition()
    {
        return HasTarget ? target.position : transform.position;
    }
} 