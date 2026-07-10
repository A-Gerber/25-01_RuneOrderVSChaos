using System.Collections.Generic;
using UnityEngine;

internal class ArrowFactory : MonoBehaviour
{
    [SerializeField] private ArrowPresenter _arrowPresenterPrefab;

    private List<Vector3> _directions;

    private void Awake()
    {
        _directions = new()
        {
            Vector3.forward,
            Vector3.right,
            Vector3.left,
            Vector3.back
        };
    }

    internal ArrowPresenter Create(List<LocalPosition> coordinates, SmallCubeSpawner smallCubeSpawner)
    {
        int directionIndex = 0;
        Vector3 position = GetArrowPosition(ref directionIndex, coordinates);

        ArrowPresenter arrowPresenter = Instantiate(_arrowPresenterPrefab, position, Quaternion.identity);
         arrowPresenter.Initialize(new Arrow(_directions[directionIndex], arrowPresenter.transform, smallCubeSpawner));
        return arrowPresenter;
    }

    private Vector3 GetArrowPosition(ref int directionIndex, List<LocalPosition> coordinates)
    {
        int index = Random.Range(0, coordinates.Count);
        directionIndex = Random.Range(0, _directions.Count);

        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 randomVector = new(coordinates[index].X, 0f, coordinates[index].Z);

            if (IsIntersectByDirection(directionIndex, coordinates, randomVector) == false)
                return randomVector;
            else
                index = ++index % coordinates.Count;
        }

        return Vector3.zero;
    }

    private bool IsIntersectByDirection(int directionIndex, List<LocalPosition> coordinates, Vector3 RandomVector)
    {
        bool isIntersection = false;

        Vector3 vector = RandomVector + _directions[directionIndex];

        foreach (var coordinate in coordinates)
        {
            if (UserUtilities.IsEqualVector3(vector, new Vector3(coordinate.X, 0f, coordinate.Z)))
                isIntersection = true;
        }

        return isIntersection;
    }
}