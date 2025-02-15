using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Controls/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> HealEvent;
    public event Action<bool> ShootEvent;
    public event Action JumpEvent;
    public event Action ActivateShieldEvent;
    public event Action PauseEvent;

    public Vector2 MovementValue { get; private set; }
    public Vector2 AimValue { get; private set; }

    private Controls _controls;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimValue = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootEvent?.Invoke(true);
        }
        if (context.canceled)
        {
            ShootEvent?.Invoke(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        JumpEvent?.Invoke();
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HealEvent?.Invoke(true);
        }
        if (context.canceled)
        {
            HealEvent?.Invoke(false);
        }
    }

    public void OnActivateShield(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ActivateShieldEvent?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        PauseEvent?.Invoke();
    }
}
