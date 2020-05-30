using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AppController : MonoBehaviour
{
    /// PlayerInput is made for actual players and not Rewired-style "System Player"
    /// They own devices and in our case, would reserve the keyboard, preventing the actual Player from using it
    /// Therefore, we use a custom InputAction here and bind the perform event manually instead of using input messages.
    [SerializeField, Tooltip("Input action to toggle fullscreen")]
    private InputAction toggleFullscreenInputAction = new InputAction();

    private void Awake()
    {
        toggleFullscreenInputAction.performed += OnToggleFullscreen;
    }

    private void OnEnable()
    {
        toggleFullscreenInputAction.Enable();
    }

    private void OnDisable()
    {
        toggleFullscreenInputAction.Disable();
    }

    private void OnToggleFullscreen(InputAction.CallbackContext ctx)
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.LogFormat("Toggled fullscreen: {0}", Screen.fullScreen);
    }
}
