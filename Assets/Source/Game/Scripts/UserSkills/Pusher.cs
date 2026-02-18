using System;
using System.Collections.Generic;
using UnityEngine;

public class Pusher
{
    private float _heightOfForceImpact;

    public Pusher(float heightOfForceImpact)
    {
        _heightOfForceImpact = heightOfForceImpact;
    }

    internal void Push(List<Cube> targets, Vector3 targetPosition, float force)
    {
        targetPosition.y = _heightOfForceImpact;

        foreach (var cube in targets)        
            cube.PushAtPoint(targetPosition, force);        
    }
}