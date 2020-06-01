using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using CommonsDebug;
using UnityConstants;

public class FirstPersonInteractor : MonoBehaviour
{
    /* Children references */
    
    public Camera firstPersonCamera;
    
    
    /* Parameters */
    
    [Tooltip("Mouse texture to use when not hovering mouse over interactable object (Free cursor only)")]
    public Texture2D defaultCursor;

    [SerializeField, Tooltip("Hotspot for default cursor")]
    protected Vector2 defaultHotspot;

    [Tooltip("Mouse texture to use when hovering mouse over interactable object (Free cursor only)")]
    public Texture2D hoverCursor;

    [SerializeField, Tooltip("Hotspot for hover cursor")]
    protected Vector2 hoverHotspot;
    
    [SerializeField, Tooltip("Cursor mode to use when changing cursor. Use ForceSoftware if, when moving/looking away" +
                             "from an interactable hover area using keyboard/gamepad motion only, the cursor doesn't refresh.")]
    private CursorMode cursorMode = CursorMode.Auto;
    
    [SerializeField, Tooltip("Max distance (m) over which the character can interact with an object")]
    private float maxInteractDistance = 2f;
    
    
    /* State vars */

    /// Can the character interact now?
    private bool m_CanInteract;
    
    /// Interactable currently hovered (cleared on interaction start)
    private Interactable m_HoveredInteractable;
    
    /// Object currently interacted with
    private Interactable m_ActiveInteractable;


    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        SetCanInteractInternal(true);
        
        m_HoveredInteractable = null;
        m_ActiveInteractable = null;
        
        ResetCursor();
    }
    
    private void OnEnable()
    {
        DialogueManager.onDialogueStarted += OnDialogueStarted;
        DialogueManager.onDialogueEnded += OnDialogueEnded;
        
        GameplayEventManager.onMasterEventStarted += OnMasterEventStarted;
        GameplayEventManager.onMasterEventEnded += OnMasterEventEnded;
        
        InGameMenu.menuOpened += OnMenuOpened;
        InGameMenu.menuClosed += OnMenuClosed;
    }

    private void OnDisable()
    {
        DialogueManager.onDialogueStarted -= OnDialogueStarted;
        DialogueManager.onDialogueEnded -= OnDialogueEnded;
        
        GameplayEventManager.onMasterEventStarted -= OnMasterEventStarted;
        GameplayEventManager.onMasterEventEnded -= OnMasterEventEnded;
        
        InGameMenu.menuOpened -= OnMenuOpened;
        InGameMenu.menuClosed -= OnMenuClosed;
    }

    private void Update()
    {
        // detect interactable under cursor every frame
        // no performance issue found, but if any, just do it every 5 frames or so (and/or when moving)
        if (m_CanInteract)
        {
            Interactable interactable = null;
            
            // use actual cursor position, since in unlocked cursor mode it's not always the screen center
            Ray mouseRay = firstPersonCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            // detect Interactable objects with rays blocked by various obstacles
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, maxInteractDistance, 
                Layers.InteractableMask | Layers.EnvironmentMask))
            {
                // don't fail if there is no component, as it must simply be an Environment object
                interactable = hitInfo.collider.GetComponent<Interactable>();
            }

            if (m_HoveredInteractable != interactable)
            {
                if (m_HoveredInteractable == null)
                {
                    OnHoverStart();
                }
                else if (interactable == null)
                {
                    OnHoverEnd();
                }
                
                m_HoveredInteractable = interactable;
            }
        }
    }

    private void UpdateCanInteract()
    {
        bool canInteract = !GameplayEventManager.Instance.IsEventPlaying() && !InGameMenu.Instance.IsOpen();
        SetCanInteract(canInteract);
    }

    /// Set m_CanInteract and update hovered item and cursor
    private void SetCanInteract(bool newValue)
    {
        if (m_CanInteract != newValue)
        {
            SetCanInteractInternal(newValue);
        }
    }
    
    /// Like SetCanInteract, but no difference check. Useful on Setup.
    private void SetCanInteractInternal(bool newValue)
    {
        m_CanInteract = newValue;

        if (newValue)
        {
            OnCanInteractStart();
        }
        else
        {
            OnCanInteractEnd();
        }
    }
    
    private void OnCanInteractStart()
    {
        // nothing for now, the next Update will detect any interactable under the cursor anyway
    }

    private void OnCanInteractEnd()
    {
        if (m_HoveredInteractable != null)
        {
            // cancel hover detection immediately
            m_HoveredInteractable = null;
            OnHoverEnd();
        }    
    }
    
    private void OnHoverStart()
    {
        SetHoverCursor();
    }

    private void OnHoverEnd()
    {
        ResetCursor();
    }
    
    private void ResetCursor()
    {
        // set cursor image for both modes (in practice, cursor will teleport to center on lock,
        // so it's less meaningful than updating the cursor image on next frame, and only for the current mode,
        // but it is more convenient to just update everything at once)
        
        // set Free cursor texture
        Cursor.SetCursor(defaultCursor, defaultHotspot, cursorMode);
        
        CursorCanvas.Instance.SetCursorImage(isHovering: false);

        // this should only be done on Reset, and even though, it doesn't seem needed in Unity 2019 anymore
//        OnCursorReset();
    }
    
    
    private void SetHoverCursor()
    {
        // set Free cursor texture
        Cursor.SetCursor(hoverCursor, hoverHotspot, cursorMode);
        
        CursorCanvas.Instance.SetCursorImage(isHovering: true);
    }

    private void OnCursorReset()
    {
        if (cursorMode == CursorMode.Auto)
        {
            // when using Hardware cursor, cursor will not refresh when leaving hover area
            // without moving the mouse (e.g. by moving with WASD only), so we need to force refresh
            // however, this will cause a blink so we only do this if needed
            Cursor.visible = false;
            Cursor.visible = true;
        }
    }
    
    private void OnInspect(InputValue value)
    {
        if (value.isPressed && m_HoveredInteractable != null)
        {
            // Transfer hovered to active interactable
            // (do not set m_HoveredInteractable to true, we need it to pass
            // the test inside SetIsInteracting, which will then clear m_HoveredInteractable)
            m_ActiveInteractable = m_HoveredInteractable;
            
            // start interaction sequence
            m_ActiveInteractable.Interact();
        }
    }
    
    private void OnDialogueStarted()
    {
        // no need to update can interact, events include dialogue
    }

    private void OnDialogueEnded()
    {
        // no need to update can interact, events include dialogue
    }
    
    private void OnMasterEventStarted()
    {
        UpdateCanInteract();
    }
    
    private void OnMasterEventEnded()
    {
        UpdateCanInteract();
    }
    
    private void OnMenuOpened()
    {
        UpdateCanInteract();
    }

    private void OnMenuClosed()
    {
        UpdateCanInteract();
    }
}
