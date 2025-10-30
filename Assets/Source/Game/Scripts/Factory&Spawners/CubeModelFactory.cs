using RuneOrderVSChaos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class CubeModelFactory : MonoBehaviour
    {
        internal CubeModel Create(Transform transform, float durationLanding, float distanceRaycast)
        { 
            return new CubeModel(transform, durationLanding, distanceRaycast);
        }
    }
}