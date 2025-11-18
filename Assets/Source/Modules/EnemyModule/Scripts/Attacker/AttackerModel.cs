public class AttackerModel
{
    private readonly int _damagePerProjectile;
    private IDamageable _enemy;

    public AttackerModel(int damagePerProjectile)
    {
        _damagePerProjectile = damagePerProjectile;
    }

    public void SetEnemy(IDamageable enemy)
    {
        _enemy = enemy;
    }

    public void Attack(int countCells)
    {
        _enemy.TakeDamage(countCells * _damagePerProjectile);
    }
}