using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Ray _ray;

    private ShapeHandler _shapeHandler;
    private PlayerInput _playerInput;

    internal event Action UsedSkill;

    private void Awake()
    {
        _shapeHandler = new(_camera, _ray);
        _playerInput = new PlayerInput();

        _playerInput.Player.TakeShape.performed += OnTakeShape;
        _playerInput.Player.PutShape.performed += OnPutShape;
        _playerInput.Player.UseSkill.performed += OnUseSkill;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public void OnTakeShape(InputAction.CallbackContext context)
    {
        _shapeHandler.RaiseShape();
    }

    public void OnPutShape(InputAction.CallbackContext context)
    {
        _shapeHandler.PutShape();
    }

    public void OnUseSkill(InputAction.CallbackContext context)
    {
        UsedSkill?.Invoke();
    }
}