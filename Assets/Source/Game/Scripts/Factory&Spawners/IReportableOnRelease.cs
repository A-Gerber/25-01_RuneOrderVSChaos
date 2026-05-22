using System;

internal interface IReportableOnRelease
{
    event Action<int> ReleasedShape;
}