using System.Collections.Generic;
using UnityEngine;

internal class ArrowFactory : MonoBehaviour
{
    [SerializeField] private ArrowView _arrowViewPrefab;

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

    internal ArrowView Create(List<LocalPosition> coordinates, ISmallCubeSpawner smallCubeSpawner)
    {
        int directionIndex = 0;
        Vector3 position = GetArrowPosition(ref directionIndex, coordinates);

        ArrowView arrowView = Instantiate(_arrowViewPrefab, position, Quaternion.identity);
        arrowView.Initialize(new(_directions[directionIndex], arrowView.transform, smallCubeSpawner));

        return arrowView;
    }

    private Vector3 GetArrowPosition(ref int directionIndex, List<LocalPosition> coordinates)
    {
        int index = Random.Range(0, coordinates.Count);
        directionIndex = Random.Range(0, _directions.Count);       

        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector3 randomVector = new(coordinates[index].PositionX, 0f, coordinates[index].PositionZ);

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
            if (UserUtilities.IsEqualVector3(vector, new Vector3(coordinate.PositionX, 0f, coordinate.PositionZ)))
                isIntersection = true;
        }

        return isIntersection;
    }
}
