using UnityEngine;

public class SkillFactory : MonoBehaviour
{
    [SerializeField] private SkillUserView _skillUserViewPrefab;
    [SerializeField] private SkillView _skillViewPrefab;
    [SerializeField] private Transform _skillContainer;

    internal SkillUser Create(float minBorderArea, float maxBorderArea, float cameraHeight)
    {
        Skill skill = new Skill();
        Instantiate(_skillViewPrefab, _skillContainer).Initialize(skill);

        SkillUser skillUser = new(skill, minBorderArea, maxBorderArea, cameraHeight);
        Instantiate(_skillUserViewPrefab, _skillContainer).Initialize(skillUser);

        return skillUser;
    }
}
