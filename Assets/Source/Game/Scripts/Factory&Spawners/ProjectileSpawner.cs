using System;
using System.Collections.Generic;
using UnityEngine;

internal class ProjectileSpawner : Spawner<WizardProjectile>, ICreateableBullets
{
    [SerializeField] private int _damage;
    [SerializeField] private int _speed;

    private Vector3 _enemyPosition;
    private List<Vector3> _startPositions;

    private int _index = 0;

    internal void Initialize(Vector3 enemyPosition)
    {
        _enemyPosition = enemyPosition;
    }

    public void CreateBullets(List<Vector3> position)
    {
        _startPositions = position;

        for (int i = 0; i < position.Count; i++)
        {
            Get();
        }
    }

    protected override void OnRelease(WizardProjectile bullet)
    {
        if (bullet == null)
            throw new InvalidOperationException("bullet is null");

        base.OnRelease(bullet);

        bullet.Released -= Release;
    }

    protected override void OnGet(WizardProjectile bullet)
    {
        if (bullet == null)
            throw new InvalidOperationException("bullet is null");

        base.OnGet(bullet);

        bullet.Initialize(_damage, _speed, _enemyPosition);
        bullet.transform.position = _startPositions[_index];
        _index++;

        if (_index == _startPositions.Count)
            _index = 0;

        bullet.Released += Release;
    }
}