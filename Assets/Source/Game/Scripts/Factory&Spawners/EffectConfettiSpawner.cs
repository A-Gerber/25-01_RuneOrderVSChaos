using System;
using UnityEngine;

internal class EffectConfettiSpawner : Spawner<EffectConfetti>
{
    [SerializeField] private Transform[] _pointsSpawn;

    private IWinable _game;
    private int _index = 0;

    internal void Initialize(IWinable game)
    {
        if (_game != null)
            _game.GameWined -= OnWinGame;

        _game = game ?? throw new InvalidOperationException("game is null");

        _game.GameWined += OnWinGame;
    }

    internal void OnWinGame(GameSavedData data)
    {
        for (int i = 0; i < _pointsSpawn.Length; i++)
            Get();
    }

    protected override void OnRelease(EffectConfetti effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnRelease(effect);

        effect.Released -= Release;
    }

    protected override void OnGet(EffectConfetti effect)
    {
        if (effect == null)
            throw new InvalidOperationException("effect is null");

        base.OnGet(effect);

        effect.transform.position = _pointsSpawn[_index].position;
        effect.Play(_index + 1);
        _index = ++_index % _pointsSpawn.Length;

        effect.Released += Release;
    }
}