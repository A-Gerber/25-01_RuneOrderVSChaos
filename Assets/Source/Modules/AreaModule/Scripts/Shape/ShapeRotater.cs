using System;
using UnityEngine;

public class ShapeRotater
{
    private const float RotateMultiplier = 30;
    private const float ErrorRateMovement = 0.05f;
    private const float StepLerp = 5f;

    private readonly Transform _transform;
    private Vector3 _oldPosition = -Vector3.one;

    public ShapeRotater(Transform transform)
    {
        _transform = transform != null ? transform : throw new InvalidOperationException("transform is null"); ;
    }

    internal void Rotate()
    {
        Vector3 direction = _transform.position - _oldPosition;
        Vector3 movement = Vector3.zero;

        if (direction.magnitude > ErrorRateMovement)
        {
            float MovementOnZ = Mathf.Round(Vector3.Dot(Vector3.forward, direction.normalized));

            if (Mathf.Approximately(MovementOnZ, 0f))
            {
                if (Mathf.Round(Vector3.Dot(Vector3.right, direction.normalized)) >= 0)
                    movement.z = -1f;
                else
                    movement.z = 1f;
            }
            else
            {
                movement.x = MovementOnZ;
            }

            Lerp(movement * RotateMultiplier);
            _oldPosition = _transform.position;
            return;
        }

        Lerp(movement);
        _oldPosition = _transform.position;
    }

    private void Lerp(Vector3 vector)
    {
        float rotateX = Mathf.Lerp(_transform.rotation.x, vector.x, StepLerp * Time.fixedDeltaTime);
        float rotateZ = Mathf.Lerp(_transform.rotation.z, vector.z, StepLerp * Time.fixedDeltaTime);

        _transform.rotation = Quaternion.Euler(new Vector3(rotateX, 0f, rotateZ));
    }
}
