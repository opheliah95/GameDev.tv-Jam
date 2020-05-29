using System.Collections;
using System.Collections.Generic;
using CommonsDebug;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityConstants;
using UnityEngine.Serialization;

public class FirstPersonInteractor : MonoBehaviour
{
    /* External references */

    [Tooltip("Mouse image to use when not hovering mouse over interactable object")]
    public Image defaultCursorImage;
    
    [Tooltip("Mouse image to use when hovering mouse over interactable object")]
    public Image hoverCursorImage;
    
    
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

    /// Is the character inspecting something?
    private bool m_IsInteracting;
    
    /// Is a cinematic playing (automated dialogue sequence preventing other interactions)?
    private bool m_IsCinematicPlaying;
    
    /// Derived attribute: can the character interact now?
    private bool m_CanInteract;
    
    /// Interactable currently hovered (cleared on interaction start)
    private Interactable m_HoveredInteractable;
    
    /// Object currently interacted with
    private Interactable m_ActiveInteractable;


    private void Awake()
    {
        Debug.Assert(defaultCursorImage != null, "No defaultCursorImage defined", this);
        Debug.Assert(hoverCursorImage != null, "No hoverCursorImage defined", this);
    }
    
    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        m_IsInteracting = false;
        m_IsCinematicPlaying = false;
        UpdateCanInteract();  // to be rigorous, but currently does nothing
        
        m_HoveredInteractable = null;
        m_ActiveInteractable = null;
        
        ResetCursor();
    }
    
    private void OnEnable()
    {
        DialogueManager.onDialogueEnded += OnDialogueEnded;
    }

    private void OnDisable()
    {
        DialogueManager.onDialogueEnded -= OnDialogueEnded;
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
    

    private void SetIsInteracting(bool value)
    {
        m_IsInteracting = value;
        UpdateCanInteract();
    }
    
    private void SetIsCinematicPlaying(bool value)
    {
        m_IsCinematicPlaying = value;
        UpdateCanInteract();
    }
    
    private void UpdateCanInteract()
    {
        bool newValue = !m_IsInteracting && !m_IsCinematicPlaying;
        if (m_CanInteract != newValue)
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
        
        // set Locked cursor image
        defaultCursorImage.enabled = true;
        hoverCursorImage.enabled = false;
        
        OnCursorChange();
    }
    
    
    private void SetHoverCursor()
    {
        // set Free cursor texture
        Cursor.SetCursor(hoverCursor, hoverHotspot, cursorMode);
        
        // set Locked cursor image
        defaultCursorImage.enabled = false;
        hoverCursorImage.enabled = true;
        
        OnCursorChange();
    }

    private void OnCursorChange()
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
            // transfer hovered to active interactable
            m_ActiveInteractable = m_HoveredInteractable;
            m_HoveredInteractable = null;
            
            // flag we are interacting to prevent further interaction and update cursor
            // (this also normally clears the hovered interactable, but it's already done here)
            SetIsInteracting(true);
            
            // start interaction sequence
            m_ActiveInteractable.Interact();
        }
    }

    private void OnDialogueEnded()
    {
        // for now, we only leave interacting state when dialogue is over
        // later, we'll have a broader variety of interaction events and will need to check for the
        // end of the current event sequence
        SetIsInteracting(false);
    }
}
