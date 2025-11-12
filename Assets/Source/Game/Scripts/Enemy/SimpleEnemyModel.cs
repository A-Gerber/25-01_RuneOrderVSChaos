using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class SimpleEnemyModel : IEnemy
    {
        private int _health;

        internal SimpleEnemyModel(int health, Vector3 position)
        {
           if(health <= 0)
                throw new ArgumentOutOfRangeException(nameof(health));

            MaxHealth = health;
            SetMaxHealth();
            Position = position;
        }

        internal event Action<int> ChangedHealth;

        internal int MaxHealth { get; private set; }
        internal Vector3 Position { get; private set; }

        public bool IsAlive => _health > 0;

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));

            if (IsAlive)
            {
                _health -= damage;

                if (_health < 0) 
                    _health = 0;

                ChangedHealth?.Invoke(_health);
            }
        }

        public void Restart()
        {
            SetMaxHealth();
            ChangedHealth?.Invoke(_health);
        }

        internal void SetMaxHealth()
        {
            _health = MaxHealth;
        }
    }
}