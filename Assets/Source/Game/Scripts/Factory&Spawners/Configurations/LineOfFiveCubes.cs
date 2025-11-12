namespace RuneOrderVSChaos
{
    internal class LineOfFiveCubes : CubesConfiguration
    {
        private const bool IsUseCoefficients = false;
        private const bool IsTransposing = true;

        private readonly int[,] _startConfiguration;

        internal LineOfFiveCubes()
        {
            _startConfiguration = new int[,] {
                { 1 },
                { 1 },
                { 1 },
                { 1 },
                { 1 }
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
}