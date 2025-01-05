using UnityEngine;



    public interface IInputHandler
    {
        Vector3 GetMovementInput();
        bool GetJumpInput();
    } 