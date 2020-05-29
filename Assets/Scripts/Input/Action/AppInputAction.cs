// GENERATED AUTOMATICALLY FROM 'Assets/Input/App.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @App : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @App()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""App"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""94cc21e5-c4d3-4449-a812-f491b4b0d6ce"",
            ""actions"": [
                {
                    ""name"": ""ToggleFullscreen"",
                    ""type"": ""Button"",
                    ""id"": ""5b8282d5-2ff5-4b59-9d49-323b99c6f3e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7254cdc0-8592-43c8-91e1-7fc552495dec"",
                    ""path"": ""<Keyboard>/f11"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Alt+Enter"",
                    ""id"": ""e6daae99-ddcb-454e-9e84-2c00c9150448"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFullscreen"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""17e03c34-7268-4f69-9b78-c485074cdf3d"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""ef69abc4-2572-425c-add8-0f6c963855af"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFullscreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_ToggleFullscreen = m_Global.FindAction("ToggleFullscreen", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Global
    private readonly InputActionMap m_Global;
    private IGlobalActions m_GlobalActionsCallbackInterface;
    private readonly InputAction m_Global_ToggleFullscreen;
    public struct GlobalActions
    {
        private @App m_Wrapper;
        public GlobalActions(@App wrapper) { m_Wrapper = wrapper; }
        public InputAction @ToggleFullscreen => m_Wrapper.m_Global_ToggleFullscreen;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void SetCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterface != null)
            {
                @ToggleFullscreen.started -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleFullscreen;
                @ToggleFullscreen.performed -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleFullscreen;
                @ToggleFullscreen.canceled -= m_Wrapper.m_GlobalActionsCallbackInterface.OnToggleFullscreen;
            }
            m_Wrapper.m_GlobalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ToggleFullscreen.started += instance.OnToggleFullscreen;
                @ToggleFullscreen.performed += instance.OnToggleFullscreen;
                @ToggleFullscreen.canceled += instance.OnToggleFullscreen;
            }
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    public interface IGlobalActions
    {
        void OnToggleFullscreen(InputAction.CallbackContext context);
    }
}
