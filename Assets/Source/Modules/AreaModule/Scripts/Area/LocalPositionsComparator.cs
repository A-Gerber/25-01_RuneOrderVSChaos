public static class LocalPositionsComparator
{
    public static bool IsEqualPosition(LocalPosition firstPosition, LocalPosition secondPosition)
    {
        return firstPosition.PositionX == secondPosition.PositionX && firstPosition.PositionZ == secondPosition.PositionZ;
    }
}