using System.Collections.Generic;
using UnityEngine;

public interface IPlayFieldSkillContactable
{
    public bool TryIdentifyTargets(List<LocalPosition> coordinates, Vector3 forceImpactPosition);
}