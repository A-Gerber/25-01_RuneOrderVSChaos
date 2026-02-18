using System;
using System.Collections;
using UnityEngine;

public class GroundImpactEffect : MonoBehaviour
{
    [SerializeField] private Transform _projectile;
    [SerializeField] private Transform _rune;
    [SerializeField] private float _projectileFlightTime = 0.5f;
    [SerializeField] private float _timeMultiplayer = 3f;

    private WaitForSeconds _wait;
    private MoverTo _mover;

    public event Action<GroundImpactEffect> Released;

    private void Awake()
    {
        _wait = new WaitForSeconds(_projectileFlightTime * _timeMultiplayer);
        _mover = new MoverTo(_projectile);
    }

    public void Perform(Vector3 targetPosition)
    {
        _projectile.localPosition = Vector3.zero;
        _projectile.LookAt(targetPosition);

        _rune.position = targetPosition;
        _mover.MoveTo(targetPosition, _projectileFlightTime);

        StartCoroutine(ReleaseOverTime());
    }

    private IEnumerator ReleaseOverTime()
    {
        yield return _wait;
        Released?.Invoke(this);
    }
}
