using System;
using UnityEngine;

internal class GroundImpactEffectSpawner : Spawner<GroundImpactEffect>
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    internal void SetStartPosition(Vector3 position)
    {
        _startPosition = position;
    }

    internal void CreateEffect(Vector3 position)
    {
        _targetPosition = position;

        Get();
    }

    protected override void OnRelease(GroundImpactEffect effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnRelease(effect);

        effect.Released -= Release;
    }

    protected override void OnGet(GroundImpactEffect effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnGet(effect);
        effect.transform.position = _startPosition;
        effect.Perform(_targetPosition);

        effect.Released += Release;
    }
}