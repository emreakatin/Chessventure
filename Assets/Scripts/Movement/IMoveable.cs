using UnityEngine;

public interface IMoveable
{
    void Move(Vector3 direction);
    void Rotate(Vector3 direction);

    public bool IsMoving();
    public bool IsRunning();
} 