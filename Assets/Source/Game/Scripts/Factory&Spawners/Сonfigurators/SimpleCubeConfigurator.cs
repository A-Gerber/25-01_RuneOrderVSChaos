using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class SimpleCubeConfigurator : ICubeConfigurator
    {

        public List<Vector3> CreateConfiguration()
        {
            List<Vector3> coordinate = new()
            {
                new Vector3(-1.5f, 0, -0.5f),
                new Vector3(-0.5f, 0, -0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(1.5f, 0, -0.5f)
            };

            return coordinate;
        }
    }
}