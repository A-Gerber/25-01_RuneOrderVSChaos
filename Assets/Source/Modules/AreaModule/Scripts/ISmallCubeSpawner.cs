using System.Collections.Generic;
using UnityEngine;

public interface ISmallCubeSpawner
{
    void Create(Vector3 positions);

    void Release();
}