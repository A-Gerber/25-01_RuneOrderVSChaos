internal interface IFactoryData
{
    ShapePresenterSpawner ShapePresenterSpawner { get; }
    EnemiesFactory EnemiesFactory { get; }
    ICreateableBullets ProjectileSpawner { get; }
    PlayerInputController PlayerInputController { get; }
    IChangeableLevel UserSkillHandler { get; }
}