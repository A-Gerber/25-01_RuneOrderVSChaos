using System;
using UnityEngine;

internal class StalactiteViewSpawner : Spawner<StalactiteView>
{
    private Vector3 _position;

    internal event Action<StalactiteView> GetedStalactiteView;

    internal void CreateStalactite(Vector3 position)
    {
        _position = position;

        Get();
    }

    protected override StalactiteView Create()
    {
        StalactiteView @object = Instantiate(Prefab);
        @object.Initialize(new Stalactite());

        return @object;
    }

    protected override void OnRelease(StalactiteView stalactite)
    {
        if (stalactite == null)
            throw new InvalidOperationException("effect is null");

        base.OnRelease(stalactite);

        stalactite.Released -= Release;
    }

    protected override void OnGet(StalactiteView stalactite)
    {
        if (stalactite == null)
            throw new InvalidOperationException("effect is null");

        base.OnGet(stalactite);
        stalactite.transform.position = _position;
        GetedStalactiteView?.Invoke(stalactite);

        stalactite.Released += Release;
    }
}
