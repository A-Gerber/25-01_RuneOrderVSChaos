using System;
using UnityEngine;

public class MoverBehindCursor
{
    private readonly float _gridStep;
    private readonly float _speed;

    private Vector3 _offset = Vector3.zero;
    private Vector3 _gridOffset = Vector3.zero;
    private Vector3 _flightOffset = new (0, Constants.FlightAltitude, 0);
    //private Plane _plane = new(Vector3.up, new Vector3(0, Constants.FlightAltitude, 0));
    private Plane _plane = new(Vector3.up, Vector3.zero);

    public MoverBehindCursor(float gridStep, float speed)
    {
        if (gridStep < 0)
            throw new ArgumentOutOfRangeException(nameof(gridStep));

        if (speed <= 0)
            throw new ArgumentOutOfRangeException(nameof(speed));

        _gridStep = gridStep;
        _speed = speed;
    }

    virtual internal void CalculateOffset(Vector3 cubePosition)
    {
        _offset = -cubePosition;
    }

    virtual internal void Move(Transform transform, float verticalShift)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(ray, out float distance) == false)
            return;

        Vector3 intersection = ray.GetPoint(distance);

        MoveOnGridWithTransition(transform, verticalShift, intersection);

        //if (UserUtilities.IsLocateInArena(intersection))
        //{
        //    MoveOnGridWithTransition(transform, verticalShift, intersection);
        //}
        //else
        //{
        //    transform.position = intersection + _offset + _flightOffset);
        //}
    }

    private void MoveOnGridWithTransition(Transform transform, float verticalShift, Vector3 intersection)
    {
        //Vector3 gridPosition = new(Mathf.Floor(intersection.x) + _gridStep, intersection.y, Mathf.Floor(intersection.z) + _gridStep);
        Vector3 gridPosition = new(Mathf.Round(intersection.x) + _gridStep, intersection.y, Mathf.Round(intersection.z) + _gridStep);

        Vector3 targetPosition = gridPosition + _offset + _flightOffset + Vector3.forward * verticalShift + _gridOffset;

        if ((transform.position - targetPosition).sqrMagnitude < Constants.CloseDistance)
        {
            if (!UserUtilities.IsEqualVector3(transform.position, targetPosition))
                transform.position = targetPosition;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, targetPosition, _speed * Time.deltaTime);
        }
    }
}

public class MobileMoverBehindCursor : MoverBehindCursor
{
    public MobileMoverBehindCursor(float gridStep, float speed) : base(gridStep, speed)
    { }

    override internal void CalculateOffset(Vector3 cubePosition)
    { }
}