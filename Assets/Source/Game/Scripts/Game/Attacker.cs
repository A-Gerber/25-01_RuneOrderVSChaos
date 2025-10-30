using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class Attacker
    {
        internal void Attack(IDamageable enemy,int damage) 
        {
            if (enemy == null)
                throw new InvalidOperationException("enemy is null");

            enemy.TakeDamage(damage);
        }
    }
}