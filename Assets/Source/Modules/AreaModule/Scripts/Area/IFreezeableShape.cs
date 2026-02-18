using UnityEngine;

public interface IFreezeableShape
{
    bool TryFreezeRandomShape(ref Vector3 position);
}
