using System;
using System.Collections.Generic;

internal class SkillsSavedData
{
    private readonly List<string> _activatedSkills = new();

    public SkillsSavedData(List<string> activatedSkills)
    {
        if (activatedSkills == null)
            throw new InvalidOperationException("activatedSkills is null");

        _activatedSkills.AddRange(activatedSkills);
    }

    internal List<string> GetActivatedtSkills()
    {   
        return _activatedSkills;
    }
}
