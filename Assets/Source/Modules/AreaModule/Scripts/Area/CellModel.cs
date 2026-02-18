using System;

public class CellModel : ITakeable
{
    private IReleaseable _item;
    private bool _isBusy = false;
    private LocalPosition _position;

    public CellModel(LocalPosition position)
    {
        _position = position;
    }

    internal event Action ChangedDisplayRune;

    public LocalPosition Position => _position;

    public bool IsBusy => _isBusy;
    internal bool IsBusyByStalactite => _item is Stalactite;
    internal bool IsEnableRune { get; private set; } = true;

    public void Take(IReleaseable item)
    {
        _item = item ?? throw new InvalidOperationException("item is null");
        _isBusy = true;
    }

    internal void EnableRune()
    {
        if (IsEnableRune == false)
        {
            IsEnableRune = true;
            ChangedDisplayRune?.Invoke();
        }
    }

    internal void DisableRune()
    {
        if (IsEnableRune)
        {
            IsEnableRune = false;
            ChangedDisplayRune?.Invoke();
        }
    }

    internal IReleaseable GetItemWhenRestarting()
    {
        _isBusy = false;
        return _item;
    }

    internal IReleaseable GetItem()
    {
        return _item;
    }

    internal void Release—ell()
    {
        _isBusy = false;
    }
}
