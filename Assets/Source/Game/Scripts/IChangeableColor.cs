using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal interface IChangeableColor
    {
        void ChangeColorCells();
    }

    internal interface IProcessable
    {
        void ProcessStep();
    }

    internal interface ICubeConfigurator
    {
        List<LocalPosition> CreateConfiguration();
    }

    internal interface IConfiguration
    {
    }

    internal interface IDamageable
    {
        void TakeDamage(int damage);
    }

    internal interface IRestartable
    {
        void Restart();
    }

    internal interface IEnemy: IDamageable, IRestartable
    {
        bool IsAlive {  get; }
    }

    internal interface IWinnable
    {
        void Win();
    }

    internal interface ICreateableBullets
    {
        void CreateBullets(List<Vector3> position);
    }
}