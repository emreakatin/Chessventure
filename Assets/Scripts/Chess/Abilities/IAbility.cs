public interface IAbility
{
    float Cooldown { get; }
    bool IsReady { get; }
    void Activate();
} 