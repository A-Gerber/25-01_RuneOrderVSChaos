using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal interface IChangeableColor
    {
        void ChangeColorCell();
    }

    internal interface IDiagnosticable
    {
        void PerformDiagnostics();
    }

    internal interface IAttackable
    {
        void Attack(int damage);
    }

    internal interface IGame : IDiagnosticable, IAttackable
    {
    }

    internal interface ICubeConfigurator
    {
        List<Vector3> CreateConfiguration();
    }

    internal interface IDamageable
    {
        void TakeDamage(int damage);
    }
}