using UnityEngine;

internal class AttackerFactory : MonoBehaviour
{
    private const int NumberSimpleCombo = 1;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private AttackerPresenter _presenterPrefab;
    [SerializeField] private AttackerView _viewPrefab;
    [SerializeField] private Transform _container;
    [SerializeField] private RectTransform _attackerViewContainer;
    [SerializeField] private float _perlinNoiseTimeScale = 1f;
    [SerializeField] private AnimationCurve _perlinNoiseAmplitudeCurve;

    private AttackerView _attackerView;
    private ScoreCounterPresenter _scoreCounterPresenter;

    internal AttackerPresenter AttackerPresenter { get; private set; }

    internal Attacker Create(IGetableEnemy enemiesFactory, ProjectileSpawner projectileSpawner, IncreasedDamageScreen screen)
    {
        _attackerView = Instantiate(_viewPrefab, _attackerViewContainer);
        _scoreCounterPresenter = _attackerView.transform.GetComponent<ScoreCounterPresenter>();

        ScoreCounter counter = new(NumberSimpleCombo);
        Attacker attacker = new(enemiesFactory, projectileSpawner, counter);
        CameraShaker cameraShaker = new(_cameraTransform, _perlinNoiseTimeScale, _perlinNoiseAmplitudeCurve);
        _scoreCounterPresenter.Initialize(counter);
        AttackerPresenter = Instantiate(_presenterPrefab, _container);
        AttackerPresenter.Initialize(attacker, _attackerView, cameraShaker, screen);

        return attacker;
    }
}