using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event Action OnPauseAction;
    public event Action OnJumpAction;
    [SerializeField] private Camera _camera;
    private PlayerInput _playerInput;
    private void Awake()
    {
        _playerInput = new();
        _playerInput.Player.Pause.performed += Pause_performed;
        _playerInput.Player.Jump.performed += Jump_performed;
    }
    private void Start()
    {
        DeliveryManager.Instance.OnStartedToDeliver += Instance_OnStartedToDeliver;
        Finish.OnFinishLineReached += Finish_OnFinishLineReached;
    }
    private void Finish_OnFinishLineReached()
    {
        _playerInput.Disable();
    }
    private void OnDestroy()
    {
        DeliveryManager.Instance.OnStartedToDeliver -= Instance_OnStartedToDeliver;
        Finish.OnFinishLineReached -= Finish_OnFinishLineReached;
    }
    private void Instance_OnStartedToDeliver()
    {
        _playerInput.Enable();
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        return _playerInput.Player.Movement.ReadValue<Vector2>().normalized;
    }
}
