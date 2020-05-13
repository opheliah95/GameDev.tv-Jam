using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
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
        /*
#if UNITY_EDITOR_LINUX
        // in Linux editor, mouse get captured and you can't move at all,
        // so you need to press Escape to move, despite having some cursor offset
        // and risking to leave the viewport... we might as well not lock cursor at all
        Cursor.lockState = CursorLockMode.None;
#else
        Cursor.lockState = CursorLockMode.Locked;
#endif
*/
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
        float mouseX = cameraRotation.x * mouseSensitivity * Time.deltaTime;
        float mouseY = cameraRotation.y * mouseSensitivity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        trans.Rotate(Vector3.up * mouseX);
    }

    private void OnLook(InputValue value)
    {
        cameraRotation = value.Get<Vector2>();
    }

    private void OnMovement(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void OnRun(InputValue value)
    {
        // input is binary anyway, so just check for any threshold between 0 and 1
        isRunning = value.isPressed;
        Debug.LogFormat("isRunning: {0}", isRunning);
    }
}
