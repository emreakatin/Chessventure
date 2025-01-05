using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour, IMoveable
{
    protected Character character;
    protected float currentMovementSpeed;
    protected float currentRotationSpeed;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public abstract void Move(Vector3 direction);
    public abstract void Rotate(Vector3 direction);

    public void UpdateMovementData(CharacterData data)
    {
        currentMovementSpeed = data.movementSpeed;
        currentRotationSpeed = data.rotationSpeed;
    }

    protected float GetMovementSpeed() => currentMovementSpeed;
    protected float GetRotationSpeed() => currentRotationSpeed;

    // Abstract method for checking if the character is moving
    public abstract bool IsMoving();

    // Abstract method for checking if the character is running
    public abstract bool IsRunning();
} 