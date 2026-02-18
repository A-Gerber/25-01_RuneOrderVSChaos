using System;
using UnityEngine;

public class SmallCube : MonoBehaviour
{
    public event Action<SmallCube> Released;

    public void Release()
    {
        Released?.Invoke(this);
    }
}
