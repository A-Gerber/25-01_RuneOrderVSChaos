internal class TConfiguration : CubesConfiguration
{
    private const bool IsUseCoefficients = true;
    private const bool IsTransposing = true;

    private readonly int[,] _startConfiguration;

    internal TConfiguration()
    {
        _startConfiguration = new int[,] {
                { 1, 1, 1},
                { 0, 1, 0},
                { 0, 1, 0},
                { 0, 1, 0}
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