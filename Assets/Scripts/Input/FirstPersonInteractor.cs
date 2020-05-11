using System.Collections;
using System.Collections.Generic;
using CommonsDebug;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityConstants;

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
    
    [SerializeField, Tooltip("Max distance (m) over which the character can interact with an object")]
    private float m_MaxInteractDistance = 2f;
    
    
    /* State vars */

    /// Is the character inspecting something?
    private bool m_IsInteracting;
    
    /// Is a cinematic playing (automated dialogue sequence preventing other interactions)?
    private bool m_IsCinematicPlaying;
    
    /// Derived attribute: can the character interact now?
    private bool m_CanInteract;
    
    /// Interactable currently hovered
    private Interactable m_HoveredInteractable;

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
        UpdateCanInteract();
        
        m_HoveredInteractable = null;
        
        ResetCursor();
    }
    
    private void Update()
    {
        if (m_CanInteract)
        {
            Interactable interactable = null;
            
            // detect interactable under cursor every frame
            // OLD: actual cursor position
//            Ray mouseRay = firstPersonCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            // NEW: always center position (should be the same when cursor is Locked)
            Ray mouseRay = firstPersonCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hitInfo;
            
            // detect Interactable objects with rays blocked by various obstacles
            if (Physics.Raycast(mouseRay, out hitInfo, m_MaxInteractDistance, 
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
        // set Locked cursor image
        defaultCursorImage.enabled = true;
        hoverCursorImage.enabled = false;
    }

    private void SetHoverCursor()
    {
        // set Locked cursor image
        defaultCursorImage.enabled = false;
        hoverCursorImage.enabled = true;
    }
    
    private void OnInspect(InputValue value)
    {
        if (value.isPressed && m_HoveredInteractable != null)
        {
            m_HoveredInteractable.Interact();
        }
    }
}
