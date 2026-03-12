using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Ray _ray;
    [SerializeField] private float _delayBeforeLifting = 0.2f;

    private ShapeLifter _shapeLifter;
    private PlayerInput _playerInput;
    private WaitForSeconds _delay;
    private bool _canRaise = true;

    internal event Action UsedSkill;

    private void Awake()
    {
        _shapeLifter = new ShapeLifter(_camera, _ray);
        _playerInput = new PlayerInput();
        _delay = new WaitForSeconds(_delayBeforeLifting);

        _playerInput.Player.TakeShape.started += OnTakeShape;
        _playerInput.Player.PutShape.performed += OnPutShape;
        _playerInput.Player.UseSkill.performed += OnUseSkill;

        _playerInput.TouchControls.TakeShape.performed += OnTakeShape;
        _playerInput.TouchControls.PutShape.canceled += OnPutShape;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnDestroy()
    {
        _playerInput.Player.TakeShape.started -= OnTakeShape;
        _playerInput.Player.PutShape.performed -= OnPutShape;
        _playerInput.Player.UseSkill.performed -= OnUseSkill;

        _playerInput.TouchControls.TakeShape.performed -= OnTakeShape;
        _playerInput.TouchControls.PutShape.canceled -= OnPutShape;
    }

    public void OnTakeShape(InputAction.CallbackContext context)
    {
        if(_canRaise)
        {
            StartCoroutine(ChangeLiftingStatusOverTime());
            _shapeLifter.LiftShape();
        }
    }

    public void OnPutShape(InputAction.CallbackContext context)
    {
        _shapeLifter.PutShape();
    }

    public void OnUseSkill(InputAction.CallbackContext context)
    {
        UsedSkill?.Invoke();
    }

    private IEnumerator ChangeLiftingStatusOverTime()
    {
        _canRaise = false;
        yield return _delay;
        _canRaise = true;
    }
}