using UnityEngine;

public interface ILiftable
{
    bool IsRaised { get; }

    void SetStatusRaised(Vector3 cube);

    void Put();
}