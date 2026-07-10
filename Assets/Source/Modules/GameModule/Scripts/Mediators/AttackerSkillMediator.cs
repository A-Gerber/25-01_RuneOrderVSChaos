using System;
using UnityEngine;

internal class AttackerSkillMediator : MonoBehaviour
{
    private IAttackerSkillContactable _attacker;
    private ISkillAttackerContactable _userSkillPerformer;

    internal void Initialize(IAttackerSkillContactable attacker, ISkillAttackerContactable userSkillPerformer)
    {
        if (_userSkillPerformer != null)
        {
            _userSkillPerformer.SettingAttacker -= (skill) => {if (enabled) _attacker?.SetParameters(skill.DamagePerProjectile, skill.ComboSkillPointsInterval, skill.TimeFrameOfCombo);};
            _userSkillPerformer.UsingSkillForAttacker -= (count) => {if (enabled) _attacker.DamageWithSkill(count);};
        }

        if (_attacker != null)
            _attacker.RewardingManaUserSkillPerformer -= (numberOfRewards) => {if (enabled) _userSkillPerformer?.RewardWithMana(new ComboManaReward(numberOfRewards));};

        _attacker = attacker ?? throw new ArgumentNullException("attacker is null", nameof(attacker));
        _userSkillPerformer = userSkillPerformer ?? throw new ArgumentNullException("userSkillPerformer is null", nameof(userSkillPerformer));

        if (_userSkillPerformer != null)
        {
            _userSkillPerformer.SettingAttacker += (skill) => { if (enabled) _attacker?.SetParameters(skill.DamagePerProjectile, skill.ComboSkillPointsInterval, skill.TimeFrameOfCombo); };
            _userSkillPerformer.UsingSkillForAttacker += (count) => { if (enabled) _attacker.DamageWithSkill(count); };
        }

        if (_attacker != null)
            _attacker.RewardingManaUserSkillPerformer += (numberOfRewards) => { if (enabled) _userSkillPerformer?.RewardWithMana(new ComboManaReward(numberOfRewards)); };
    }
}
