using System;

internal interface ISettingableSkillButton
{
    event Action OpenedSkillsMenu;

    void SetFirstUserSkill(UserSkill skill);

    void SetSecondUserSkill(UserSkill skill);

    void SetThirdUserSkill(UserSkill skill);

    void ResetSkillButtons();
}