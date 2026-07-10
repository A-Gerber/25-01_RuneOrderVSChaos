internal class CubeState
{
    private bool _value;

    public CubeState(bool value)
    {
        _value = value;
    }

    internal bool Value => _value;

    internal void SetValue(bool value)
    {
        _value = value;
    }
}