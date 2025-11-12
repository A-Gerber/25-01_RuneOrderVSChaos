using System;
using System.Collections.Generic;

namespace RuneOrderVSChaos
{
    internal class CubeViewSpawner : Spawner<CubeView>
    {
        private List<LocalPosition> _coordinates;
        private List<CubeView> _currentCubeViews = new List<CubeView>();
        private int _index = 0;

        internal event Action<List<CubeView>> CreatedCubeView;

        internal void CreateCubes(List<LocalPosition> coordinates)
        {
            if (coordinates == null)
                throw new InvalidOperationException("coordinate is null");

            _coordinates = coordinates;

            for (int i = 0; i < coordinates.Count; i++)
            {
                Get();
            }
        }

        protected override CubeView Create()
        {
            CubeView @object = Instantiate(Prefab);
            @object.Initialize(new CubeModel(@object.transform, @object.DurationLanding, @object.DistanceRaycast));

            return @object;
        }

        protected override void OnRelease(CubeView cube)
        {
            if (cube == null)
                throw new InvalidOperationException("cube is null");

            base.OnRelease(cube);

            cube.Released -= Release;
        }

        protected override void OnGet(CubeView cube)
        {
            if (cube == null)
                throw new InvalidOperationException("cube is null");

            base.OnGet(cube);

            cube.SetLocalPosition(_coordinates[_index]);
            _currentCubeViews.Add(cube);
            SendCubeViews();

            cube.Released += Release;
        }

        private void SendCubeViews()
        {
            if (_index == _coordinates.Count - 1)
            {
                CreatedCubeView?.Invoke(_currentCubeViews);
                _index = 0;
                _currentCubeViews.Clear();
            }
            else
            {
                _index++;
            }
        }
    }
}