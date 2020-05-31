using CommonsHelper;
using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    /* Sibling components */

    private Transform trans;
    private CharacterController controller;
    private PlayerInput playerInput;
    
    
    /* Child references */
    
    [SerializeField, Tooltip("Player camera")]
    private Camera playerCamera = null;
    
    
    /* Parameters */
    
    [SerializeField, Tooltip("How much the Free cursor must be close to the screen's left/right to trigger yaw rotation (ratio of screen width)")]
    private float freeCursorRotateScreenRatioMarginX = 0.2f;

    [SerializeField, Tooltip("How fast the the camera rotates on yaw when the cursor is completely on the left/right edge of the screen (in deg/s)")]
    private float freeCursorMaxRotateYawSpeed = 90f;

    [SerializeField, Tooltip("How much the Free cursor must be close to the screen's up/down to trigger pitch rotation (ratio of screen height)")]
    private float freeCursorRotateScreenRatioMarginY = 0.2f;

    [SerializeField, Tooltip("How fast the the camera rotates on pitch when the cursor is completely on the left/right edge of the screen (in deg/s)")]
    private float freeCursorMaxRotatePitchSpeed = 90f;

    [SerializeField, Tooltip("Absolute margin to ignore mouse near the screen edges to avoid rotation stuck when mouse leaves screen (px)")]
    private float freeCursorScreenEdgeDeadMargin = 10f;
    
    
    /* State vars */
    
    /// Is the cursor is lock mode?
    private bool m_IsCursorLocked;
    
    /// Tracked cursor lock state to restore after menu choice
    private bool m_WasCursorLockedBeforeMenu;
    
    public float walkSpeed = 3f, runSpeed = 6f;
    public float mouseSensitivity = 100f;
    public float gamepadKeyboardSensitivity = 100f;
    float xRotation = 0;
    
    /// Tracks if a dialogue is active
    /// Superseded by GameplayEventManager.Instance.IsEventPlaying, which contains both dialogue and cinematic sequences
    public static bool isTalking = false;

    /// Last move intention
    private Vector2 move = Vector2.zero;

    /// Accumulated mouse movement to rotate camera
    private Vector2 cameraMouseRotation = Vector2.zero;

    /// Camera rotation input by gamepad right stick or keyboard arrows
    private Vector2 cameraGamepadKeyboardRotation = Vector2.zero;

    /// Is the character running?
    private bool isRunning;

    private void Awake()
    {
        trans = transform;
        controller = this.GetComponentOrFail<CharacterController>();
        playerInput = this.GetComponentOrFail<PlayerInput>();
    }

    private void Start()
    {
#if UNITY_EDITOR
        // In Linux editor, mouse get captured and you can't move at all,
        // so you need to press Escape to move, despite having some cursor offset
        // and risking to leave the viewport... we might as well not lock cursor at all.
        // In Windows editor, it's OK but may be annoying, so we also start in unlocked mode.
        SetCursorLockInternal(false);
#else
        // start with cursor locked
        // use internal version to make sure all members are updated according to initial value
        SetCursorLockInternal(true);
#endif
    }
    
    private void OnEnable()
    {
        InGameMenu.menuOpened += OnMenuOpened;
        InGameMenu.menuClosed += OnMenuClosed;
        
        GameplayEventManager.onMasterEventStarted += OnMasterEventStarted;
        GameplayEventManager.onMasterEventEnded += OnMasterEventEnded;
    }

    private void OnDisable()
    {
        InGameMenu.menuOpened -= OnMenuOpened;
        InGameMenu.menuClosed -= OnMenuClosed;
        
        GameplayEventManager.onMasterEventStarted -= OnMasterEventStarted;
        GameplayEventManager.onMasterEventEnded -= OnMasterEventEnded;
    }
    
    private void Update()
    {
        // IsEventPlaying should cover isTalking anyway
        if (!isTalking && !GameplayEventManager.Instance.IsEventPlaying())
        {
            playerMove();
            playerLook();
        }
    }

    void playerMove()
    {
        float xMovement = move.x;
        float yMovement = move.y;
        float tempSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(trans.right * xMovement * tempSpeed * Time.deltaTime);
        controller.Move(trans.forward * yMovement * tempSpeed * Time.deltaTime);
    }

    void playerLook()
    {
        // initialize yaw and pitch
        float yRotationDelta = 0f;
        float xRotationDelta = 0f;
        
        if (m_IsCursorLocked)
        {
            // use accumulated rotation motion (we'll consume it at the bottom of this method so it's also
            // cleared if cursor is unlocked)
            yRotationDelta = cameraMouseRotation.x * mouseSensitivity * Time.deltaTime;
            xRotationDelta = cameraMouseRotation.y * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            // Viewport is clamped, so check if mouse is inside game window with mouse position directly.
            // Unfortunately, mouse position will only be tracked one frame when leaving the screen
            //  sometimes, the first position outside the screen won't be caught at all,
            //  and the mouse position will be stuck, causing uninterrupted motion while mouse is outside window.
            // Therefore, I add a small margin near the exact edge of the screen where I will ignore rotation
            // due to mouse position.
            // This margin must be small, so we don't care adapting the boundaries of the Lerp to max rotate speed
            // In fullscreen, this is not a problem so ignore the margin.
            // Unity 2019 Editor seems to be considered fullscreen, so for the editor just enable the margin
#if UNITY_EDITOR
            float deadMargin = freeCursorScreenEdgeDeadMargin;
#else
            float deadMargin = Screen.fullScreen ? 0f : freeCursorScreenEdgeDeadMargin;
#endif
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            if (mousePosition.x >= deadMargin && mousePosition.x <= Screen.width - deadMargin &&
                mousePosition.y >= deadMargin && mousePosition.y <= Screen.height - deadMargin)
            {
                // rotate when mouse is near the screen edge, a la StarCraft
                // (I would have preferred a Confined cursor where extra delta farther than the edge move,
                // but Confined doesn't work correctly, at least in Linux builds)
                Vector3 viewportPoint = playerCamera.ScreenToViewportPoint(mousePosition);

                // on left and side edges, change yaw
                float ratioMarginX = Mathf.Clamp(freeCursorRotateScreenRatioMarginX, 0.01f, 0.5f);
                if (viewportPoint.x < ratioMarginX)
                {
                    yRotationDelta = - Mathf.Lerp(freeCursorMaxRotateYawSpeed, 0f, viewportPoint.x / ratioMarginX);
                }
                else
                {
                    float relativeMousePosX = viewportPoint.x - (1f - ratioMarginX);
                    if (relativeMousePosX > 0)
                    {
                        yRotationDelta = Mathf.Lerp(0f, freeCursorMaxRotateYawSpeed, relativeMousePosX / ratioMarginX);
                    }
                }
                yRotationDelta *= Time.deltaTime;
                
                // on top and bottom edges, change pitch
                float ratioMarginY = Mathf.Clamp(freeCursorRotateScreenRatioMarginY, 0.01f, 0.5f);
                if (viewportPoint.y < ratioMarginY)
                {
                    xRotationDelta = - Mathf.Lerp(freeCursorMaxRotatePitchSpeed, 0f, viewportPoint.y / ratioMarginY);
                }
                else
                {
                    float relativeMousePosY = viewportPoint.y - (1f - ratioMarginY);
                    if (relativeMousePosY > 0)
                    {
                        xRotationDelta = Mathf.Lerp(0f, freeCursorMaxRotatePitchSpeed, relativeMousePosY / ratioMarginY);
                    }
                }
                xRotationDelta *= Time.deltaTime;
            }
        }
        
        // consume/clear rotation accumulated over the last frames
        cameraMouseRotation = Vector2.zero;
        
        // apply gamepad/keyboard look input independently from cursor mode
        yRotationDelta += cameraGamepadKeyboardRotation.x * gamepadKeyboardSensitivity * Time.deltaTime;
        xRotationDelta += cameraGamepadKeyboardRotation.y * gamepadKeyboardSensitivity * Time.deltaTime;
        
        // cumulate pitch
        xRotation -= xRotationDelta;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // apply yaw on body, but pitch on camera head
        trans.Rotate(Vector3.up * yRotationDelta);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }
    
    
    /* Cursor mode */
    
    private void SetCursorLock(bool value)
    {
        if (m_IsCursorLocked != value)
        {
            SetCursorLockInternal(value);
        }
    }

    private void SetCursorLockInternal(bool value)
    {
        m_IsCursorLocked = value;
        
        if (value)
        {
            // avoid flicker by hiding early
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CursorCanvas.Instance.SetCanvasEnable(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CursorCanvas.Instance.SetCanvasEnable(false);
        }
    }

    private void ToggleCursorLock()
    {
        SetCursorLockInternal(!m_IsCursorLocked);
    }
    
    
    /* Event callbacks */
    
    private void OnMenuOpened()
    {
        m_WasCursorLockedBeforeMenu = m_IsCursorLocked;
        SetCursorLock(false);
    }

    private void OnMenuClosed()
    {
        SetCursorLock(m_WasCursorLockedBeforeMenu);
    }
    
    private void OnMasterEventStarted()
    {
        // Known issue: switching to UI then back to Gameplay
        // loses the EventSystem UI bindings, almost disabling normal button click in the in-game menu
        // after an event has happened
        // So for now, comment out the switch and keep the default Gameplay + magic event system click enabled
        // Instead, use runtime flag test to check if events are running and we shouldn't move, etc.
//        playerInput.SwitchCurrentActionMap("UI");
    }
    
    private void OnMasterEventEnded()
    {
//        playerInput.SwitchCurrentActionMap("Gameplay");
    }
    
    
    /* Input Action callbacks */

    private void OnLook(InputValue value)
    {
        // set rotation for gamepad/keyboard
        cameraGamepadKeyboardRotation = value.Get<Vector2>();
    }

    private void OnLookDelta(InputValue value)
    {
        // accumulate rotation (may be called multiple times between Updates)
        cameraMouseRotation += value.Get<Vector2>();
    }

    private void OnMovement(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        isRunning = value.isPressed;
        Debug.LogFormat("isRunning: {0}", isRunning);
    }
    
    private void OnToggleCursorLock(InputValue value)
    {
        Debug.Assert(value.isPressed, "OnToggleCursorLock received value not isPressed, make sure not to set " +
                                      "ToggleCursorLock interaction to Pass Through nor Press and Release so it only detects press.");
        ToggleCursorLock();
    }
}
