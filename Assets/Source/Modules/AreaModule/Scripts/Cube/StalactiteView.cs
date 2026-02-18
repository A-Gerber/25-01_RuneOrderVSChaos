using System;
using UnityEngine;

public class StalactiteView : MonoBehaviour
{
    private Stalactite _stalactite;

    public event Action<StalactiteView> Released;

    private void OnEnable()
    {
        if (_stalactite != null)
            _stalactite.Released += OnRelease;
    }

    private void OnDisable()
    {
        if (_stalactite != null)
            _stalactite.Released -= OnRelease;
    }

    public void Initialize(Stalactite stalactite)
    {
        if (_stalactite != null)
            _stalactite.Released -= OnRelease;

        _stalactite = stalactite ?? throw new InvalidOperationException("stalactite is null");

        _stalactite.Released += OnRelease;
    }

    public IReleaseable GetStalactite()
    {
        return _stalactite;
    }

    private void OnRelease()
    {
        Released?.Invoke(this);
    }
}