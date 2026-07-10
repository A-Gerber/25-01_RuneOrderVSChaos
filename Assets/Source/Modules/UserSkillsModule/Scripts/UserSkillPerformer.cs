using System;
using System.Collections.Generic;
using UnityEngine;

public class UserSkillPerformer : ISkillAttackerContactable, ISkillRuneBoardContactable, ISkillShapeSpawnerContactable, IReportableOnUsedSkill
{
    private UserSkillHandler _userSkillHandler;
    private ManaGenerator _manaGenerator;
    private ParticleSystem _hintAboutUsingSkill;
    private IIdentifiableTargets _mediator;

    public event Action<IPassiveSkill> SettingAttacker;
    public event Action<int> UsingSkillForAttacker;
    public event Action<List<LocalPosition>> UsingSkillForShapeSpawner;
    public event Action RuneBoardChecking;
    public event Action RuneBoardAttacking;
    public event Action RuneBoardReleasingTargets;
    public event Action UsedSkill;
    internal event Action Started;

    public int ManaCount => _manaGenerator.ManaCount;
    public int ManaCountIncrease => _manaGenerator.ManaCountIncrease;
    public bool HaveManaForSkill => _manaGenerator.HaveManaForSkill();
    public int ManaIncreaseForADV => _manaGenerator.CalculateIncrease();
    internal int ManaCostPerLevel => _manaGenerator.ManaCostPerLevel;

    public void Start(UserSkillSavedData savedData, int level)
    {
        _manaGenerator.SetStartData(level, savedData.ManaCount);
        _userSkillHandler.StartGame(savedData.GetActivatedSkills(), level);
        ChangeStateHint(false);
        Started?.Invoke();
    }

    public void Restart()
    {
        _manaGenerator.Restart();
        ChangeStateHint(false);
        Started?.Invoke();
    }

    public void GoToNextLevel(UserSkillSavedData savedData, int level)
    {
        _userSkillHandler.StartGame(savedData.GetActivatedSkills(), level);
        ChangeStateHint(false);
        Started?.Invoke();
    }

    public List<string> GetSkillsToSave()
    {
        return _userSkillHandler.GetSkillsToSave();
    }

    public void RewardWithMana(ManaReward reward)
    {
        _manaGenerator.Reward(reward);
    }

    public void ChangeStateHint(bool isEnabled)
    {
        _hintAboutUsingSkill.gameObject.SetActive(isEnabled);
    }

    internal void Initialize(UserSkillHandler userSkillHandler, ManaGenerator manaGenerator, ParticleSystem hintAboutUsingSkill, IIdentifiableTargets mediator)
    {
        _userSkillHandler = userSkillHandler ?? throw new ArgumentNullException("userSkillHandler is null", nameof(userSkillHandler));
        _manaGenerator = manaGenerator ?? throw new ArgumentNullException("manaGenerator is null", nameof(manaGenerator));
        _hintAboutUsingSkill = hintAboutUsingSkill != null ? hintAboutUsingSkill : throw new ArgumentNullException("hintAboutUsingSkill is null", nameof(hintAboutUsingSkill));
        _mediator = mediator ?? throw new ArgumentNullException("mediator is null", nameof(mediator));
    }

    internal void CheckOverGame()
    {
        RuneBoardChecking?.Invoke();
    }

    internal bool CanSpendMana(int cost)
    {
        return _manaGenerator.CanSpendMana(cost);
    }

    internal void UseSecondPartOfSkill(UserSkill skill)
    {
        if (skill is ISettableInFirstButton)
        {
            RuneBoardReleasingTargets?.Invoke();
        }
        else if (skill is ISettableInSecondButton)
        {
            RuneBoardAttacking?.Invoke();
        }

        CheckOverGame();
    }

    internal void UseFirstPartOfSkill(UserSkill skill, Vector3 targetPosition)
    {
        if (skill is ISettableInThirdButton)
        {
            UsingSkillForAttacker?.Invoke(skill.SkillDamage);
            SpendManaAndPlayEffects(skill, targetPosition);
            return;
        }

        LocalPosition position = new ((int)Mathf.Round(targetPosition.x), (int)Mathf.Round(targetPosition.z));
        List<LocalPosition> skillCoordinates = skill.GetSkillCoordinates(position, (int)Mathf.Round(Constants.MinBorderArea), (int)Mathf.Round(Constants.MaxBorderArea));

        if (skill is ISettableInSecondButton)
        {
            UsingSkillForShapeSpawner?.Invoke(skillCoordinates);
            SpendManaAndPlayEffects(skill, targetPosition);
            return;
        }

        if (_mediator.TryIdentifyTargets(skillCoordinates, targetPosition))
            SpendManaAndPlayEffects(skill, targetPosition);
    }

    private void SpendManaAndPlayEffects(UserSkill skill, Vector3 targetPosition)
    {
        _manaGenerator.SpendMana(skill.ManaCost);
        skill.PlayEffects(targetPosition);
        UsedSkill?.Invoke();
    }

    internal void Set(IPassiveSkill passiveSkill)
    {
        _manaGenerator.SetComboManaReward(passiveSkill.ComboManaReward);
        SettingAttacker?.Invoke(passiveSkill);
    }
}
