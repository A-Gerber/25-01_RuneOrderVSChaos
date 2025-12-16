using System;
using UnityEngine;

public class SkillView : MonoBehaviour
{
    private UserSkill _skill;

    public void Initialize(UserSkill skill)
    {
        if (_skill != null)
            _skill.Used -= OnUsed;

        _skill = skill ?? throw new InvalidOperationException("skill is null");

        _skill.Used += OnUsed;
    }

    private void OnUsed()
    {
        Debug.Log("Сделать отображение");
    }
}