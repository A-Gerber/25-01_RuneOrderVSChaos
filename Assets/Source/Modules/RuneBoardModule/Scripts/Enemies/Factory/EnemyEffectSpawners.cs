internal class EnemyEffectSpawner
{
    public EnemyEffectSpawner(StalactiteViewSpawner stalactiteSpawner, GroundImpactEffectSpawner groundImpactEffectSpawner, FreezingEffectSpawner freezingEffectSpawner)
    {
        StalactiteSpawner = stalactiteSpawner;
        GroundImpactEffectSpawner = groundImpactEffectSpawner;
        FreezingEffectSpawner = freezingEffectSpawner;
    }

    internal StalactiteViewSpawner StalactiteSpawner { get; private set; }
    internal GroundImpactEffectSpawner GroundImpactEffectSpawner { get; private set; }
    internal FreezingEffectSpawner FreezingEffectSpawner { get; private set; }
}