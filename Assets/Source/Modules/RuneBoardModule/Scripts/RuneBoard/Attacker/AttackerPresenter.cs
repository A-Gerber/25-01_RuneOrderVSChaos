using System;
using UnityEngine;

internal class AttackerPresenter : MonoBehaviour, IAttackerPresenter
{
    [SerializeField] private float _amplitude = 5f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _shakeMultiplier = 2f;

    private IncreasedDamageScreen _screen;
    private Attacker _attacker;
    private AttackerView _attackerView;
    private CameraShaker _cameraShaker;

    public event Action RewardButtonClicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WizardProjectile bullet))
        {
            bullet.Release();

            if (_attacker.CanAttack)
                _attacker.Damage();
        }
    }

    private void Update()
    {
        _cameraShaker?.UpdateShake();
    }

    public void RewardDamage(int value)
    {
        _attacker.SetRewardMultiplier(value);
    }

    internal void Initialize(Attacker attacker, AttackerView attackerView, CameraShaker cameraShaker, IncreasedDamageScreen screen)
    {
        if (_attacker != null)
        {
            _attacker.Damaged -= (count) => { if (enabled) _attackerView.OnShowScored(count); };
            _attacker.ChangeMultiplier -= () => { if (enabled) _attackerView.OnShowMultiplier(_attacker.DamageMultiplier); };
            _attacker.ShakedCamera -= () => { if (enabled) _cameraShaker.MakeShake(_amplitude, _duration * _shakeMultiplier); };
        }

        if (_attackerView != null)
            _attackerView.OpenButtonClicked -= () => { if (enabled) _screen.Open(); };

        if (_screen != null)
            _screen.RewardButtonClicked -= () => { if (enabled) RewardButtonClicked?.Invoke(); };

        _attacker = attacker ?? throw new ArgumentNullException("attacker is null", nameof(attacker));
        _attackerView = attackerView != null ? attackerView : throw new ArgumentNullException("attackerView is null", nameof(attackerView));
        _cameraShaker = cameraShaker ?? throw new ArgumentNullException("cameraShaker is null", nameof(cameraShaker));
        _screen = screen != null ? screen : throw new ArgumentNullException("screen is null", nameof(screen));

        if (_attacker != null)
        {
            _attacker.Damaged += (count) => { if (enabled) _attackerView.OnShowScored(count); };
            _attacker.ChangeMultiplier += () => { if (enabled) _attackerView.OnShowMultiplier(_attacker.DamageMultiplier); };
            _attacker.ShakedCamera += () => { if (enabled) _cameraShaker.MakeShake(_amplitude, _duration * _shakeMultiplier); };
        }

        if (_attackerView != null)
            _attackerView.OpenButtonClicked += () => { if(enabled) _screen.Open(); };

        if (_screen != null)
            _screen.RewardButtonClicked += () => { if (enabled) RewardButtonClicked?.Invoke(); };
    }
}
