using System;
using UnityEngine;

internal class FreezingEffectSpawner : Spawner<FreezingEffect>
{
    private Vector3 _targetPosition;

    internal void CreateEffect(Vector3 position)
    {
        _targetPosition = position;

        Get();
    }

    protected override void OnRelease(FreezingEffect effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnRelease(effect);

        effect.Released -= Release;
    }

    protected override void OnGet(FreezingEffect effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnGet(effect);
        effect.transform.position = _targetPosition;
        effect.Perform();

        effect.Released += Release;
    }
}