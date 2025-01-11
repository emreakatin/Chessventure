using UnityEngine;

[RequireComponent(typeof(Player), typeof(CharacterController))]
public class PlayerMovement : CharacterMovement
{
    private CharacterController controller;
    private IInputHandler inputHandler;
    private Vector3 verticalVelocity;
    private int currentJumpCount;
    private IHealthSystem healthSystem;


    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<IInputHandler>();
        healthSystem = GetComponent<IHealthSystem>();
    }

    private void Update()
    {
        if (!healthSystem.IsDead)
        {
            HandleMovement();
            HandleGravity();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = inputHandler.GetMovementInput();
        
        if (moveDirection != Vector3.zero)
        {
            Move(moveDirection);
            Rotate(moveDirection);
        }
    }

    private void HandleGravity()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
            currentJumpCount = 0;
        }

        verticalVelocity.y += character.CharacterData.gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (inputHandler.GetJumpInput() && CanJump())
        {
            Jump();
        }
    }

    private bool CanJump()
    {
        return currentJumpCount < character.CharacterData.maxJumpCount;
    }

    private void Jump()
    {
        float jumpHeight = character.CharacterData.jumpHeight;
        float gravity = character.CharacterData.gravity;
        verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        currentJumpCount++;
    }

    public override void Move(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            float speed = IsRunning() ? currentMovementSpeed * 1.5f : currentMovementSpeed;
            Vector3 move = direction.normalized * speed * Time.deltaTime;
            controller.Move(move);
        }
    }

    public override void Rotate(Vector3 direction)
    {
        if (direction.magnitude > 0)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
        }
    }

    public override bool IsMoving()
    {
        // Player'ın hareket edip etmediğini kontrol et
        return inputHandler.GetMovementInput().magnitude > 0.1f;
    }

    public override bool IsRunning()
    {
        // Koşma durumu için bir kontrol ekle
        //Debug.Log(Input.GetKey(KeyCode.LeftShift));
        return Input.GetKey(KeyCode.LeftShift) && IsMoving();
    }
} 