using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _glowEffect;
    [SerializeField] private ParticleSystem _lightningFlow;
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Renderer _icingRenderer;
    [SerializeField] private Transform _icing;
    [SerializeField] private float _durationLanding = 0.3f;
    [SerializeField] private float _raycastDistance = 5f;

    private Cube _cube;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private MoverTo _moverTo;
    private TransparentChanger _transparenteChanger;
    private Quaternion _startRotation;

    private bool _isTransparent = false;

    public event Action<CubeView> Released;

    public float RaycastDistance => _raycastDistance;
    public Rigidbody Rigidbody => _rigidbody;
    public Vector3 LocalPosition { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _startRotation = _transform.rotation;
        _lightningFlow.gameObject.SetActive(false);
        _moverTo = new MoverTo(_transform);
        _transparenteChanger = new TransparentChanger(_cubeRenderer, _icingRenderer);
    }

    private void Update()
    {
        _moverTo.Move();
    }

    public float GetCubeSize()
    {
        return GetComponent<BoxCollider>().size.x;
    }

    public void Initialize(Cube cube)
    {
        if (_cube != null)
        {
            _cube.Released -= OnRelease;
            _cube.ChangedFreeze -= OnChangeFreeze;
            _cube.ChangedGlowEffect -= OnChangeGlowEffect;
            _cube.ChangedTransparente -= OnChangeTransparent;
            _cube.Pushed -= OnPush;
            _cube.Landed -= OnLand;
        }

        _cube = cube ?? throw new InvalidOperationException("cube is null");

        _cube.Released += OnRelease;
        _cube.ChangedFreeze += OnChangeFreeze;
        _cube.ChangedGlowEffect += OnChangeGlowEffect;
        _cube.ChangedTransparente += OnChangeTransparent;
        _cube.Pushed += OnPush;
        _cube.Landed += OnLand;
    }

    public void SetLocalPosition(LocalPosition position)
    {
        _cube.SetLocalPosition(position);

        LocalPosition = new Vector3(position.PositionX, 0, position.PositionZ);
    }

    public void Reset()
    {
        _isTransparent = false;
        _rigidbody.isKinematic = true;
        _transform.rotation = _startRotation;
        _lightningFlow.gameObject.SetActive(false);
    }

    public Cube GetCubeModel()
    {
        return _cube;
    }

    private void OnLand(Vector3 target)
    {
        _moverTo.SetTarget(target, _durationLanding);
    }

    private void OnRelease()
    {
        _moverTo.Reset();
        Released?.Invoke(this);
    }

    private void OnPush()
    {
        _lightningFlow.gameObject.SetActive(true);
        _lightningFlow.Play();
    }

    private void OnChangeGlowEffect(bool isNormalSize)
    {
        _glowEffect.gameObject.SetActive(isNormalSize);
    }

    private void OnChangeFreeze()
    {
        if (_cube.IsFrozen)
        {
            _icing.gameObject.SetActive(true);

            if (_isTransparent)
                ChangeTransparent();
        }
        else
        {
            _icing.gameObject.SetActive(false);
        }
    }

    private void OnChangeTransparent(bool isTransparent)
    {
        _isTransparent = isTransparent;
        ChangeTransparent();
    }

    private void ChangeTransparent()
    {
        _transparenteChanger.ChangeTransparent(_isTransparent, _cube.IsFrozen);
    }
}