using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(DisplayStateChanger))]
public class CubePresenter : MonoBehaviour
{
    [SerializeField] private ParticleSystem _glowEffect;
    [SerializeField] private ParticleSystem _lightningFlow;
    [SerializeField] private float _durationLanding = 0.3f;

    private Cube _cube;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private MoverTo _moverTo;
    private DisplayStateChanger _displayStateChanger;
    private Quaternion _startRotation;

    public event Action<CubePresenter> Released;

    public Rigidbody Rigidbody => _rigidbody;
    internal Cube CubeModel => _cube;
    public float CubeSize => GetComponent<BoxCollider>().size.x;


    private void Awake()
    {
        _transform = transform;
        _displayStateChanger = GetComponent<DisplayStateChanger>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _startRotation = _transform.rotation;
        _lightningFlow.gameObject.SetActive(false);
        _moverTo = new MoverTo(_transform);
    }

    private void Update()
    {
        _moverTo.Move();
    }

    internal void Initialize(Cube cube)
    {
        if (_cube != null)
        {
            _cube.Released -= OnRelease;
            _cube.Pushed -= OnPush;
            _cube.Landed -= OnLand;
            _cube.ChangingState -= OnChangeState;
        }

        _cube = cube ?? throw new InvalidOperationException("cube is null");

        if (_cube != null)
        {
            _cube.Released += OnRelease;
            _cube.Pushed += OnPush;
            _cube.Landed += OnLand;
            _cube.ChangingState += OnChangeState;
        }
    }

    internal void SetToDefault()
    {
        _rigidbody.isKinematic = true;
        _transform.rotation = _startRotation;
        _lightningFlow.gameObject.SetActive(false);
        _glowEffect.gameObject.SetActive(false);
        _moverTo.Reset();
    }

    private void OnLand(Vector3 target)
    {
        if (enabled)
            _moverTo.SetTarget(target, _durationLanding);
    }

    private void OnRelease()
    {
        if (enabled)
            Released?.Invoke(this);
    }

    private void OnPush()
    {
        if (enabled)
        {
            _lightningFlow.gameObject.SetActive(true);
            _lightningFlow.Play();
        }
    }

    internal void OnChangeState(CubeState state)
    {
        if (state == null)
            throw new ArgumentNullException("state is null", nameof(state));

        if (enabled == false)
            return;

        switch (state)
        {
            case SmallState smalledState:
                _glowEffect.gameObject.SetActive(!smalledState.Value);
                break;

            case FrozenState frozenState:
                _displayStateChanger.ChangeFreeze(frozenState.Value);
                break;

            case TransparencyState transparencyState:
                _displayStateChanger.ChangeTransparent(transparencyState.Value);
                break;

            default:
                break;
        }
    }
}