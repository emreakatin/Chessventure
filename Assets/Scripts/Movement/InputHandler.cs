using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    public Vector3 GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return new Vector3(horizontal, 0, vertical).normalized; // Normalize ederek yönü döndür
    }

    public bool GetJumpInput()
    {
        return Input.GetButtonDown("Jump"); // Zıplama girişi
    }
} 