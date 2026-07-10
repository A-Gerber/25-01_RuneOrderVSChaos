using UnityEngine;

internal interface IShapeFreezable
{
    public bool TryFreezeRandomShape(ref Vector3 position, FrozenState state);
}