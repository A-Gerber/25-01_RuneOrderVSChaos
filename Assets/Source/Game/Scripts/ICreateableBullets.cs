using System.Collections.Generic;
using UnityEngine;

internal interface ICreateableBullets
{
    void CreateBullets(List<Vector3> position);
}