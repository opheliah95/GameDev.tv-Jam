using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AppController : MonoBehaviour
{
    void OnToggleFullscreen(InputValue value)
    {
        Debug.Assert(value.isPressed, "OnToggleFullscreen received value not isPressed, make sure not to set " +
                                      "ToggleFullscreen interaction to Pass Through nor Press and Release so it only detects press.");

        Screen.fullScreen = !Screen.fullScreen;
        Debug.LogFormat("Toggled fullscreen: {0}", Screen.fullScreen);
    }
}
