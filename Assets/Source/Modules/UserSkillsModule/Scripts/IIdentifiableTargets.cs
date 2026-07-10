using System.Collections.Generic;
using UnityEngine;

public interface IIdentifiableTargets
{
    public bool TryIdentifyTargets(List<LocalPosition> coordinates, Vector3 forceImpactPosition);
}