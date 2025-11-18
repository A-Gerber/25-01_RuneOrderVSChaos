internal class SquareOfNineCubes : CubesConfiguration
{
    private const bool IsUseCoefficients = false;
    private const bool IsTransposing = false;

    private readonly int[,] _startConfiguration;

    internal SquareOfNineCubes()
    {
        _startConfiguration = new int[,] {
                { 1, 1, 1},
                { 1, 1, 1},
                { 1, 1, 1}
            };
    }

    protected override int[,] GetStartConfiguration()
    {
        return _startConfiguration;
    }

    protected override bool IsCalculateCoefficients()
    {
        return IsUseCoefficients;
    }

    protected override bool IsTranspose()
    {
        return IsTransposing;
    }
}