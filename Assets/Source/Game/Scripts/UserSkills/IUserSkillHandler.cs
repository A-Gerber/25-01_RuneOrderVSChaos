using System;

internal interface IUserSkillHandler : IChangeableLevel, ISaveableSkills
{

}

internal interface IChangeableLevel
{
    void StartGame(SkillsSavedData data);

    void ChangeLevel(int level);

    void Reset();
}

internal interface ISaveableSkills
{
    event Action<SkillsSavedData> SavedSkills;

    SkillsSavedData GetSkillsToSave();
}