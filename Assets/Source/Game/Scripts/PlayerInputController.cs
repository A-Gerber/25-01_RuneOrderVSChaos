using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Ray _ray;
    [SerializeField] private float _delayBeforeLifting = 0.26f;
    [SerializeField] private LayerMask _layerMask;

    private ShapeLifter _shapeLifter;
    private PlayerInput _playerInput;
    private UserSkillPerformerPresenter _userSkillPerformer;
    private WaitForSeconds _delay;
    private bool _canRaise = true;

    private void Awake()
    {
        _shapeLifter = new ShapeLifter(_camera, _ray, _layerMask);
        _playerInput = new PlayerInput();
        _delay = new WaitForSeconds(_delayBeforeLifting);

        _playerInput.Player.TakeShape.started += OnTakeShape;
        _playerInput.Player.PutShape.canceled += OnPutShape;
        _playerInput.Player.UseSkill.performed += OnUseSkill;

        _playerInput.TouchControls.TakeShape.started += OnTakeShape;
        _playerInput.TouchControls.PutShape.canceled += OnPutShape;
        _playerInput.TouchControls.UseSkill.performed += OnUseSkill;
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
        _playerInput.Player.PutShape.canceled -= OnPutShape;
        _playerInput.Player.UseSkill.performed -= OnUseSkill;

        _playerInput.TouchControls.TakeShape.started -= OnTakeShape;
        _playerInput.TouchControls.PutShape.canceled -= OnPutShape;
        _playerInput.TouchControls.UseSkill.performed -= OnUseSkill;
    }



    public void OnTakeShape(InputAction.CallbackContext context)
    {
        if (_canRaise)
        {
            StartCoroutine(ChangeLiftingStatusOverTime());
            _shapeLifter.Lift();
        }
    }

    public void OnPutShape(InputAction.CallbackContext context)
    {
        _shapeLifter.Put();
    }

    public void OnUseSkill(InputAction.CallbackContext context)
    {
        _userSkillPerformer.UseSkill();
    }

    internal void Initialize(UserSkillPerformerPresenter userSkillPerformerPresenter)
    {
        _userSkillPerformer = userSkillPerformerPresenter ?? throw new ArgumentNullException("userSkillPerformerPresenter is null", nameof(userSkillPerformerPresenter));
    }

    private IEnumerator ChangeLiftingStatusOverTime()
    {
        _canRaise = false;
        yield return _delay;
        _canRaise = true;
    }
}