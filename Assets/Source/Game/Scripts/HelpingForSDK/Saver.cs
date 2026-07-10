using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

internal class Saver : MonoBehaviour, ISaver
{
    private RuneBoardSavedData _startRuneBoardSavedData;
    private UserSkillSavedData _startUserSkillSavedData;

    public int CurrentLevel => YG2.saves.Level;
    public int GameScore => YG2.saves.GameScore;
    public int MaxGameResult => YG2.saves.MaxGameResult;
    public int ManaCount => YG2.saves.ManaCount;

    public void SetStartData(RuneBoardSavedData startRuneBoardSavedData, UserSkillSavedData startUserSkillSavedData)
    {
        _startRuneBoardSavedData = startRuneBoardSavedData ?? throw new InvalidOperationException("startGameSavedData is null");
        _startUserSkillSavedData = startUserSkillSavedData ?? throw new InvalidOperationException("startUserSkillSavedData is null");
    }

    public RuneBoardSavedData GetRuneBoardSavedData()
    {
        return new RuneBoardSavedData(YG2.saves.Level, YG2.saves.GameScore);
    }

    public RuneBoardSavedData GetStartRuneBoardSavedData()
    {
        return _startRuneBoardSavedData;
    }

    public void SaveRuneBoardData(RuneBoardSavedData gameSavedData)
    {
        if (gameSavedData == null)
            throw new InvalidOperationException("gameSavedData is null");

        YG2.saves.Level = gameSavedData.Level;
        YG2.saves.GameScore = gameSavedData.GameScore;

        if (gameSavedData.GameScore > YG2.saves.MaxGameResult)
            YG2.saves.MaxGameResult = gameSavedData.GameScore;
    }

    public void SaveUserSkillSavedData(UserSkillSavedData userSkillSavedData)
    {
        if (userSkillSavedData == null)
            throw new ArgumentNullException("userSkillSavedData is null", nameof(userSkillSavedData));

        YG2.saves.ManaCount = userSkillSavedData.ManaCount;

        YG2.saves.ActivatedSkills.Clear();
        YG2.saves.ActivatedSkills.AddRange(userSkillSavedData.GetActivatedSkills());
    }

    public void SaveManaCountOfUserSkill(int manaCount)
    {
        if (manaCount < 0)
            throw new InvalidOperationException("manaCount is not correct");

        YG2.saves.ManaCount = manaCount;
    }

    public void SaveActivatedUserSkills(List<string> activatedSkills)
    {
        if (activatedSkills == null)
            throw new ArgumentNullException("activatedSkills is null", nameof(activatedSkills));

        YG2.saves.ActivatedSkills.Clear();
        YG2.saves.ActivatedSkills.AddRange(activatedSkills);
    }

    public UserSkillSavedData GetSkillSavedData()
    {
        if (YG2.saves.ActivatedSkills == null)
            throw new ArgumentNullException("ActivatedSkills is null");

        return new UserSkillSavedData(YG2.saves.ManaCount, YG2.saves.ActivatedSkills.ToList());
    }

    public UserSkillSavedData GetStartSkillData()
    {
        return _startUserSkillSavedData;
    }

    public void Save()
    {
        YG2.SaveProgress();
    }
}