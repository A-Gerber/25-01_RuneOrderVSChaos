using System.Collections.Generic;

public interface ISaver : IGettableLevel
{
    public int GameScore { get; }
    public int MaxGameResult { get; }

    public void SetStartData(RuneBoardSavedData startRuneBoardSavedData, UserSkillSavedData startUserSkillSavedData);

    public RuneBoardSavedData GetRuneBoardSavedData();

    public RuneBoardSavedData GetStartRuneBoardSavedData();

    public void SaveRuneBoardData(RuneBoardSavedData gameSavedData);

    public UserSkillSavedData GetSkillSavedData();

    public UserSkillSavedData GetStartSkillData();

    public void SaveManaCountOfUserSkill(int manaCount);

    public void SaveActivatedUserSkills(List<string> activatedSkills);

    public void Save();
}
