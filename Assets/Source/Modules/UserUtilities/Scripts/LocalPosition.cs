public struct LocalPosition
{
    private int _positionX;
    private int _positionZ;

    public LocalPosition(int positionX, int positionZ)
    {
        _positionX = positionX;
        _positionZ = positionZ;
    }

    public int PositionX => _positionX;
    public int PositionZ => _positionZ;
}