internal class EntityDataForGame
{
    internal AreaModel AreaModel { get; private set; }
    internal AttackerModel AttackerModel { get; private set; }
    internal UserSkillPerformer UserSkillPerformer { get; private set; }
    internal EnemySkillPerfomer EnemySkillPerfomer { get; private set; }
    internal ManaGenerator ManaGenerator { get; private set; }
    internal ShapePresenterSpawner ShapePresenterSpawner { get; private set; }
    internal EnemiesFactory EnemiesFactory { get; private set; }
    internal ICreateableBullets ProjectileSpawner { get; private set; }
    internal PlayerInputController PlayerInputController { get; private set; }

    internal void Take(AreaModel areaModel, AttackerModel attacker, ManaGenerator manaGenerator)
    {
        AreaModel = areaModel;
        AttackerModel = attacker;
        ManaGenerator = manaGenerator;
    }

    internal void TakeCreators(EnemiesFactory enemiesFactory, ShapePresenterSpawner shapePresenterSpawner, ICreateableBullets projectileSpawner)
    {
        EnemiesFactory = enemiesFactory;
        ShapePresenterSpawner = shapePresenterSpawner;
        ProjectileSpawner = projectileSpawner;
    }

    internal void TakeEntityData(UserSkillPerformer userSkillPerformer, EnemySkillPerfomer enemySkillPerfomer)
    {
        UserSkillPerformer = userSkillPerformer;
        EnemySkillPerfomer = enemySkillPerfomer;
    }

    internal void TakeController(PlayerInputController controller)
    {
        PlayerInputController = controller;
    }
}
