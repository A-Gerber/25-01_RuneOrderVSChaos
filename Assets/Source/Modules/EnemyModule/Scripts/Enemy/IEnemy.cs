public interface IEnemy : IDamageable, IRestartable
{
    bool IsAlive { get; }
}
