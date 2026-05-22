using System;
using UnityEngine;

public interface IEnemy : IChangeableHealthEnemy
{
    event Action ChangedHealth;

    int Health {  get; }
    bool IsFullHealth {  get; }
    int IncreaseToHealth {  get; }
    float SkillCooldown { get; }
    Sprite Icon { get; }
    Sprite SkillIcon { get; }
    string SkillDescription { get; }

    void UpdateHealth();

    IEnemySkill GetSkill();

    void SetMaxHealth(int health);

    void TakeHealth(int health);

    void ChangeSkillDescription(Languages language);
}