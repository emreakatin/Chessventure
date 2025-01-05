using UnityEngine;

public class KeyboardInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    public Vector3 GetMovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        return new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    public bool GetJumpInput()
    {
        return Input.GetKeyDown(jumpKey);
    }
} 