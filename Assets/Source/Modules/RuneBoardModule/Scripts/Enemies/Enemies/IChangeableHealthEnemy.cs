public interface IChangeableHealthEnemy : IDamageable, IRestartable
{
    bool IsAlive { get; }
    int MaxHealth { get; }
}