public interface IPieceState
{
    void EnterState();
    void UpdateState();
    void ExitState();
    void HandleAbility();
} 