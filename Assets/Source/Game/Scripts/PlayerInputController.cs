using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Ray _ray;
    [SerializeField] private float _delayBeforeLifting = 0.4f;

    private ShapeLifter _shapeLifter;
    private PlayerInput _playerInput;
    private WaitForSeconds _delay;
    private Coroutine _coroutine;
    private bool _canRaise = true;

    internal event Action UsedSkill;

    private void Awake()
    {
        _shapeLifter = new(_camera, _ray);
        _playerInput = new PlayerInput();
        _delay = new WaitForSeconds(_delayBeforeLifting);

        _playerInput.Player.TakeShape.started += OnTakeShape;
        _playerInput.Player.PutShape.performed += OnPutShape;
        _playerInput.Player.UseSkill.performed += OnUseSkill;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _shapeLifter.Puted += ChangeLiftingStatus;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _shapeLifter.Puted -= ChangeLiftingStatus;
    }

    public void OnTakeShape(InputAction.CallbackContext context)
    {
        if(_canRaise)
            _shapeLifter.LiftShape();
    }

    public void OnPutShape(InputAction.CallbackContext context)
    {
        _shapeLifter.PutShape();
    }

    public void OnUseSkill(InputAction.CallbackContext context)
    {
        UsedSkill?.Invoke();
    }

    private void ChangeLiftingStatus()
    {
        if(_coroutine != null)
            StopCoroutine( _coroutine );

        _coroutine = StartCoroutine(ChangeLiftingStatusOverTime());
    }

    private IEnumerator ChangeLiftingStatusOverTime()
    {
        _canRaise = false;
        yield return _delay;
        _canRaise = true;
    }
}