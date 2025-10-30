using System;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class Enemy : IDamageable
    {
        private int _health;

        public Enemy(int health)
        {
            _health = health;
        }

        public void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }
    }
}