using System;
using UnityEngine;

public class StalactiteView : MonoBehaviour
{
    private Stalactite _stalactite;

    public event Action<StalactiteView> Released;

    public void Initialize(Stalactite stalactite)
    {
        if (_stalactite != null)
            _stalactite.Released -= OnRelease;

        _stalactite = stalactite ?? throw new InvalidOperationException("stalactite is null");

        if (_stalactite != null)
            _stalactite.Released += OnRelease;
    }

    public IReleasable GetStalactite()
    {
        return _stalactite;
    }

    private void OnRelease()
    {
        if (enabled)
            Released?.Invoke(this);
    }
}