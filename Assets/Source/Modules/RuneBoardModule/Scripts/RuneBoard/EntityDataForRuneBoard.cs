internal class EntityDataForRuneBoard
{
    internal PlayField PlayField { get; private set; }
    internal Attacker Attacker { get; private set; }
    internal EnemySkillPerfomer EnemySkillPerfomer { get; private set; }
    internal IRuneBoardSkillMediator Mediator { get; private set; }

    internal void TakeRuneBoardEntities(PlayField playField, Attacker attacker)
    {
        PlayField = playField;
        Attacker = attacker;
    }

    internal void Take(EnemySkillPerfomer enemySkillPerfomer, IRuneBoardSkillMediator mediator)
    {
        EnemySkillPerfomer = enemySkillPerfomer;
        Mediator = mediator;
    }
}