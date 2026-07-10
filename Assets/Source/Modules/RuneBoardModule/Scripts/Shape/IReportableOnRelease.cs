using System;

public interface IReportableOnRelease
{
    public event Action<int> ReleasedShape;
}