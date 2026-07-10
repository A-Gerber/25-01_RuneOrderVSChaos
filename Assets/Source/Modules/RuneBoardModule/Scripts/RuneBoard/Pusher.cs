using System;
using System.Collections.Generic;
using UnityEngine;

internal class Pusher
{
    private readonly float _heightOfForceImpact;
    private readonly float _forceImpact;

    internal Pusher(float heightOfForceImpact, float forceImpact)
    {
        if (forceImpact <= 0)
            throw new ArgumentOutOfRangeException(nameof(forceImpact));

        _heightOfForceImpact = heightOfForceImpact;
        _forceImpact = forceImpact;
    }

    internal void Push(List<IReleasable> targets, Vector3 forceImpactPosition)
    {
        if (targets.Count == 0)
            return;

        forceImpactPosition.y = _heightOfForceImpact;

        foreach (var target in targets)
        {
            if(target is Cube cube)
                cube.PushAtPoint(forceImpactPosition, _forceImpact);
        }
    }
}