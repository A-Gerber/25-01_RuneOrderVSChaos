using System;
using UnityEngine;

public interface IEnemy : IChangeableHealthEnemy
{
    public event Action ChangedHealth;

    public int Health {  get; }
    public bool IsFullHealth {  get; }
    public int IncreaseToHealth {  get; }
    public float SkillCoolDown { get; }
    public Sprite Icon { get; }
    public Sprite SkillIcon { get; }
    public string SkillDescription { get; }
    public IEnemySkill EnemySkill { get; }

    public void UpdateHealth();

    public void SetMaxHealth(int health);

    public void TakeHealth(int health);

    public void ChangeSkillDescription(Languages language);
}