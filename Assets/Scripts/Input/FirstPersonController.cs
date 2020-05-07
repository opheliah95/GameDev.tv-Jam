using UnityEngine.InputSystem;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    Transform trans;
    CharacterController controller;
    public float walkSpeed = 3f, runSpeed = 6f;
    public float mouseSensitivity = 100f;
    float xRotation = 0;
    public static bool isTalking;

    [SerializeField]
    Camera playerCamera;

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
        Cursor.lockState = CursorLockMode.Locked;
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
        float xMovement =  move.x;
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

    public void OnPlayerLook(InputAction.CallbackContext context)
    {
        cameraRotation = context.ReadValue<Vector2>();
    }

    public void OnPlayerMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnPlayerRun(InputAction.CallbackContext context)
    {
        isRunning = (context.phase == InputActionPhase.Performed);
    }
}
