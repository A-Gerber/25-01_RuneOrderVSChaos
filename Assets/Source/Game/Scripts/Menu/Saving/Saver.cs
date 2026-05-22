using System;
using System.Linq;
using UnityEngine;
using YG;

internal class Saver : MonoBehaviour
{
    private GameSavedData _startGameSavedData;
    private SkillsSavedData _startSkillsSavedData;

    public int CurrentLevel => YG2.saves.Level;
    public int GameScore => YG2.saves.GameScore;
    public int MaxGameResult => YG2.saves.MaxGameResult;

    internal void SetStartData(GameSavedData startGameSavedData, SkillsSavedData startSkillsSavedData)
    {
        _startGameSavedData = startGameSavedData ?? throw new InvalidOperationException("startGameSavedData is null");
        _startSkillsSavedData = startSkillsSavedData ?? throw new InvalidOperationException("startSkillsSavedData is null");
    }

    internal void SaveGameData(GameSavedData gameSavedData)
    {
        if (gameSavedData == null)
            throw new InvalidOperationException("gameSavedData is null");

        YG2.saves.Level = gameSavedData.Level;
        YG2.saves.ManaCount = gameSavedData.ManaCount;
        YG2.saves.GameScore = gameSavedData.GameScore;

        if(gameSavedData.GameScore > YG2.saves.MaxGameResult)
            YG2.saves.MaxGameResult = gameSavedData.GameScore;
    }

    internal void SaveSkillData(SkillsSavedData skillsSavedData)
    {
        if (skillsSavedData == null)
            throw new InvalidOperationException("skillsSavedData is null");

        YG2.saves.ActivatedSkills.Clear();
        YG2.saves.ActivatedSkills.AddRange(skillsSavedData.GetActivatedtSkills());
    }

    internal void Save()
    {
        YG2.SaveProgress();
    }

    internal GameSavedData GetGameSavedData()
    {
        return new GameSavedData(YG2.saves.Level, YG2.saves.ManaCount, YG2.saves.GameScore);
    }

    internal GameSavedData GetStartGameData()
    {
        return _startGameSavedData;
    }

    internal SkillsSavedData GetSkillSavedData()
    {       
        if (YG2.saves.ActivatedSkills == null)
            throw new InvalidOperationException("activatedSkills is null");

        return new SkillsSavedData(YG2.saves.ActivatedSkills.ToList());
    }

    internal SkillsSavedData GetStartSkillData()
    {
        return _startSkillsSavedData;
    }
}