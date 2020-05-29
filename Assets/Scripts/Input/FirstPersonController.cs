using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    /* External references */
    
    [Tooltip("Canvas containing cursor image")]
    public Canvas pointerCanvas;

    
    /* State vars */
    
    /// Is the cursor is lock mode?
    private bool m_IsCursorLocked;
    
    /// Tracked cursor lock state to restore after menu choice
    private bool m_WasCursorLockedBeforeMenu;
    
    
    Transform trans;
    CharacterController controller;
    public float walkSpeed = 3f, runSpeed = 6f;
    public float mouseSensitivity = 100f;
    float xRotation = 0;
    public static bool isTalking = false;

    [SerializeField, Tooltip("Player camera")]
    Camera playerCamera = null;

    [SerializeField]
    Vector2 move = Vector2.zero;

    [SerializeField]
    Vector2 cameraRotation;

    [SerializeField]
    bool isRunning;

    [SerializeField]
    float tempSpeed;

    private void Awake()
    {
        trans = transform;
        controller = GetComponent<CharacterController>();
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
    }

    private void OnDisable()
    {
        InGameMenu.menuOpened -= OnMenuOpened;
        InGameMenu.menuClosed -= OnMenuClosed;
    }
    
    private void Update()
    {
        if(!isTalking)
        {
            playerMove();
            playerLook();
        }
       
    }

    void playerMove()
    {
        float xMovement = move.x;
        float yMovement = move.y;
        tempSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(trans.right * xMovement * tempSpeed * Time.deltaTime);
        controller.Move(trans.forward * yMovement * tempSpeed * Time.deltaTime);
    }

    void playerLook()
    {
        if (m_IsCursorLocked)
        {
            float mouseX = cameraRotation.x * mouseSensitivity * Time.deltaTime;
            float mouseY = cameraRotation.y * mouseSensitivity * Time.deltaTime;
            
            // consume rotation
            cameraRotation = Vector2.zero;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            trans.Rotate(Vector3.up * mouseX);
        }
        else
        {
            
        }
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
            pointerCanvas.enabled = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pointerCanvas.enabled = false;
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
    
    
    /* Input Action callbacks */

    private void OnLook(InputValue value)
    {
        // accumulate rotation
        cameraRotation += value.Get<Vector2>();
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
        bool test = value.isPressed;
        ToggleCursorLock();
        Debug.LogFormat("Toggle Cursor Lock: {0}", test);
    }
}
