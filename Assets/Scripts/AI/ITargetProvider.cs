using UnityEngine;

public interface ITargetProvider
{
    bool HasTarget { get; }
    Vector3 GetTargetPosition();
} 