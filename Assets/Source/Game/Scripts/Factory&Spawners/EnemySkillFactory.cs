using System;
using UnityEngine;

internal class EnemySkillFactory : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _percentageOfHealing = 0.15f;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerYeti = 1;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerFenrir = 1;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerSnowQueen = 2;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerGargoyle = 3;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerEarthDragon = 2;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerWitch = 3;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerEarthWitch = 3;

    internal void SetNewSkill(IEnemy enemy)
    {
        if (enemy == null)
            throw new InvalidOperationException("enemy is null");

        if (enemy is Greenskin greenskin)
            greenskin.TakeSkill(new HealingSkill(_percentageOfHealing));
        else if (enemy is Yeti yeti)
            yeti.TakeSkill(new FreezingSkill(_numberOfUsesPerYeti));
        else if (enemy is Fenrir fenrir)
            fenrir.TakeSkill(new FreezingSkill(_numberOfUsesPerFenrir));
        else if (enemy is SnowQueen snowQueen)
            snowQueen.TakeSkill(new FreezingSkill(_numberOfUsesPerSnowQueen));
        else if (enemy is Gargoyle gargoyle)
            gargoyle.TakeSkill(new GroundImpact(_numberOfUsesPerGargoyle));
        else if (enemy is EarthDragon dragon)
            dragon.TakeSkill(new GroundImpact(_numberOfUsesPerEarthDragon));
        else if (enemy is WitchOfChaos witch)
            witch.TakeSkills(new HealingSkill(_percentageOfHealing), new FreezingSkill(_numberOfUsesPerWitch), new GroundImpact(_numberOfUsesPerEarthWitch));
    }
}