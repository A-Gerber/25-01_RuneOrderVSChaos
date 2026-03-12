using System;
using UnityEngine;
using UnityEngine.UI;

internal class EnemySkillFactory : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float _percentageOfHealing = 0.15f;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerYeti = 1;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerFenrir = 1;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerSnowQueen = 3;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerGargoyle = 1;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerEarthDragon = 3;
    [Range(1, 3)]
    [SerializeField] private int _numberOfUsesPerWitch = 3;
    [Range(1, 5)]
    [SerializeField] private int _numberOfUsesPerEarthWitch = 3;

    [SerializeField] private Sprite _healingIcon;
    [SerializeField] private Sprite _freezingIcon;
    [SerializeField] private Sprite _groundImpactIcon;
    [SerializeField] private Sprite _witchSkillIcon;

    internal void SetNewSkill(IEnemy enemy)
    {
        if (enemy == null)
            throw new InvalidOperationException("enemy is null");

        if (enemy is Greenskin greenskin)
            greenskin.TakeSkill(new HealingSkill(_percentageOfHealing, _healingIcon));
        else if (enemy is Yeti yeti)
            yeti.TakeSkill(new FreezingSkill(_numberOfUsesPerYeti, _freezingIcon));
        else if (enemy is Fenrir fenrir)
            fenrir.TakeSkill(new FreezingSkill(_numberOfUsesPerFenrir, _freezingIcon));
        else if (enemy is SnowQueen snowQueen)
            snowQueen.TakeSkill(new FreezingSkill(_numberOfUsesPerSnowQueen, _freezingIcon));
        else if (enemy is Gargoyle gargoyle)
            gargoyle.TakeSkill(new GroundImpact(_numberOfUsesPerGargoyle, _groundImpactIcon));
        else if (enemy is EarthDragon dragon)
            dragon.TakeSkill(new GroundImpact(_numberOfUsesPerEarthDragon, _groundImpactIcon));
        else if (enemy is WitchOfChaos witch)
            witch.TakeSkills(
                new HealingSkill(_percentageOfHealing, _healingIcon), 
                new FreezingSkill(_numberOfUsesPerWitch, _freezingIcon), 
                new GroundImpact(_numberOfUsesPerEarthWitch, _groundImpactIcon),
                _witchSkillIcon);
    }
}