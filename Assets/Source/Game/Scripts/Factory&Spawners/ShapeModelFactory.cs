using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeModelFactory
    {
        internal ShapeModel Create(Transform transform, ShapeMover shapeMover, float durationOfReturn)
        {
            return new ShapeModel( shapeMover, transform, durationOfReturn);
        }
    }
}