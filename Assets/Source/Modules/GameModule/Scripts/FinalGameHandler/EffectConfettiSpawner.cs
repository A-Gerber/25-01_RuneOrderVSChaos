using System;
using UnityEngine;

public class EffectConfettiSpawner : Spawner<EffectConfetti>
{
    [SerializeField] private Transform[] _pointsSpawn;

    private int _index;

    internal void CreateEffect()
    {
        for (_index = 0; _index < _pointsSpawn.Length; _index++)
            Get();
    }

    protected override void OnRelease(EffectConfetti effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnRelease(effect);

        effect.Released -= Release;
    }

    protected override void OnGet(EffectConfetti effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnGet(effect);

        effect.transform.position = _pointsSpawn[_index].position;
        effect.Play(_index + 1);

        effect.Released += Release;
    }
}
