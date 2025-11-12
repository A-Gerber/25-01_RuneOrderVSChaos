namespace RuneOrderVSChaos
{
    internal class AttackerModel
    {
        private readonly int _damagePerProjectile;
        private IDamageable _enemy;

        internal AttackerModel(int damagePerProjectile)
        {
            _damagePerProjectile = damagePerProjectile;
        }

        internal void SetEnemy (IDamageable enemy)
        {
            _enemy = enemy;
        }

        internal void Attack(int countCells)
        {
            _enemy.TakeDamage(countCells * _damagePerProjectile);
        }
    }
}