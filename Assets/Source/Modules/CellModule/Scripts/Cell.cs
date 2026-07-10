using System;

public class Cell : ITakeable
{
    private readonly LocalPosition _position;

    private IReleasable _item = null;
    internal bool _isEnableRune = false;

    public Cell(LocalPosition position)
    {
        _position = position;
    }

    internal event Action<bool> ChangedDisplayRune;

    public LocalPosition Position => _position;
    public bool IsBusy { get; private set; } = false;

    public void Take(IReleasable item)
    {
        _item = item ?? throw new ArgumentNullException("item is null", nameof(item));
        IsBusy = true;
    }

    public void ChangeRuneDisplay(bool value)
    {
        ChangedDisplayRune?.Invoke(value);
    }

    public IReleasable GetItem()
    {
        return _item;
    }

    public void Release()
    {
        IsBusy = false;
        _item = null;
    }
}
