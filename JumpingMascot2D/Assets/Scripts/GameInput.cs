using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance {  get; private set; }

    public event EventHandler<OnReleaseButtonEventArgs> OnReleaseButton;
    public event EventHandler OnHoldButton;

    public class OnReleaseButtonEventArgs: EventArgs
    {
        public float duration;
    }
    private InputActions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Player.Enable();
        inputActions.Player.Jumping.performed += Jumping_performed;
        inputActions.Player.Jumping.canceled += Jumping_canceled;
    }

    private void Jumping_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            OnHoldButton?.Invoke(this, EventArgs.Empty);

        }
    }

    private void Jumping_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            OnReleaseButton?.Invoke(this, new OnReleaseButtonEventArgs
            {
                duration = (float)obj.duration
            });
        }       
    }
}
