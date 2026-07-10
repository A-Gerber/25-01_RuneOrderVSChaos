using System;
using System.Collections.Generic;

public class UserSkillSavedData
{
    private readonly List<string> _activatedSkills = new();

    public UserSkillSavedData(int manaCount, List<string> activatedSkills)
    {
        if (manaCount < 0)
            throw new ArgumentOutOfRangeException(nameof(manaCount));

        if (activatedSkills.Count < 0)
            throw new ArgumentOutOfRangeException(nameof(activatedSkills));

        if (activatedSkills == null)
            throw new ArgumentNullException("activatedSkills is null", nameof(activatedSkills));

        ManaCount = manaCount;
        _activatedSkills.AddRange(activatedSkills);
    }

    public int ManaCount { get; private set; }

    public List<string> GetActivatedSkills()
    {
        return _activatedSkills;
    }
}