internal interface IFactoryData
{
    ShapeViewSpawner ShapeViewSpawner { get; }
    EnemiesFactory EnemiesFactory { get; }
    ICreateableBullets ProjectileSpawner { get; }
    PlayerInputController PlayerInputController { get; }
    IChangeableLevel UserSkillHandler { get; }
}