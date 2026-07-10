using System;
using System.Collections.Generic;
using UnityEngine;

internal class ProjectileSpawner : Spawner<WizardProjectile>
{
    private readonly List<Vector3> _startPositions = new();

    [SerializeField] private WizardProjectileSoundPlayer _soundPlayer;
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _speed = 10;

    private Vector3 _enemyPosition;
    private int _index = 0;

    internal void Initialize(Vector3 enemyPosition)
    {
        _enemyPosition = enemyPosition;
    }

    public void CreateBullets(List<LocalPosition> positions)
    {
        _startPositions.Clear();

        foreach (var position in positions)
            _startPositions.Add(new Vector3 (position.X, 0f, position.Z));

        _soundPlayer.PlayCreateSound();

        for (_index = 0; _index < _startPositions.Count; _index++)
            Get();
    }

    protected override void OnRelease(WizardProjectile bullet)
    {
        if (bullet == null)
            throw new InvalidOperationException("bullet is null");

        _soundPlayer.PlayDamageSound();
        base.OnRelease(bullet);

        bullet.Released -= Release;
    }

    protected override void OnGet(WizardProjectile bullet)
    {
        if (bullet == null)
            throw new InvalidOperationException("bullet is null");

        base.OnGet(bullet);

        bullet.Attack(_damage, _speed, _enemyPosition);
        bullet.transform.position = _startPositions[_index];

        bullet.Released += Release;
    }
}
