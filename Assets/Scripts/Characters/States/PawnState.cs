public class PawnState : ICharacterState
{
    private readonly Character character;
    
    public PawnState(Character character)
    {
        this.character = character;
    }

    public void EnterState()
    {
        // Piyon özelliklerini ayarla
        var data = character.CharacterData;
        data.movementSpeed *= 1.5f;  // Hızlı hareket
        data.attackRange *= 0.5f;    // Kısa menzil
    }

    public void UpdateState()
    {
        // Sürekli güncelleme gerektiren işlemler
    }

    public void HandleAbility()
    {
        // Piyona özel yetenek
    }

    public void ExitState()
    {
        // State değişiminde temizlik
    }
} 