namespace RuneOrderVSChaos
{
    internal struct LocalPosition
    {
        private int _positionX;
        private int _positionZ;

        internal LocalPosition (int positionX, int positionZ)
        {
            _positionX = positionX;
            _positionZ = positionZ;
        }

        internal int PositionX => _positionX;
        internal int PositionZ => _positionZ;
    }
}