public struct LocalPosition
{
    private readonly int _positionX;
    private readonly int _positionZ;

    public LocalPosition(int positionX, int positionZ)
    {
        _positionX = positionX;
        _positionZ = positionZ;
    }

    public readonly int X => _positionX;
    public readonly int Z => _positionZ;
}