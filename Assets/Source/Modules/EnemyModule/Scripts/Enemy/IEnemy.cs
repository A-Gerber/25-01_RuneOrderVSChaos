using System;
using UnityEngine;

public interface IEnemy : IChangeableHealthEnemy
{
    event Action ChangedHealth;
    event Action <IEnemySkill> UsedSkill;

    int Health {  get; }
    bool IsFullHealth {  get; }
    int IncreaseToHealth {  get; }
    float SkillCooldown { get; }
    Sprite Icon { get; }

    void UpdateHealth();

    void UseSkill();

    void SetMaxHealth(int health);

    void TakeHealth(int health);
}