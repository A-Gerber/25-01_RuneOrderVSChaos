using System;
using System.Collections.Generic;
using UnityEngine;

internal class PlayFieldSkillMediator : MonoBehaviour,  IIdentifiableTargets
{
    private IPlayFieldSkillContactable _playField;

    internal void Initialize(IPlayFieldSkillContactable playField)
    {
        _playField = playField ?? throw new ArgumentNullException("playField is null", nameof(playField));
    }

    public bool TryIdentifyTargets(List<LocalPosition> coordinates, Vector3 forceImpactPosition)
    {
        return _playField.TryIdentifyTargets(coordinates, forceImpactPosition);
    }
}