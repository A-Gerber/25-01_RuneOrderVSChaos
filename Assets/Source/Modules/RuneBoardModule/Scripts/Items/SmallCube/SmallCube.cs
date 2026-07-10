using System;
using UnityEngine;

internal class SmallCube : MonoBehaviour
{
    internal event Action<SmallCube> Released;

    internal void Release()
    {
        Released?.Invoke(this);
    }
}