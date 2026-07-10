using UnityEngine;

public interface ILiftable
{
    public bool IsRaised { get; }

    public void SetStatusRaised(Vector3 cube);

    public void Land();
}