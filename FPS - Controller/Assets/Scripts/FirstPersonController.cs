using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FirstPersonController : MonoBehaviour
{
    //CanMove is a boolean that determines if the player can move or not
    public bool CanMove { get; private set; } = true;

    //movement parameters
    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float gravity = 30f;

    //looking parameters
    [Header("Looking Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80f;

    //camera
    private Camera playerCamera;
    //character controller
    private CharacterController characterController;

    //vector3 for movement direction
    private Vector3 moveDirection = Vector3.zero;
    //vector2 for keyboard input
    private Vector2 currentInput = Vector2.zero;
    //x rotation
    private float rotationX = 0;
    // Start is called before the first frame update
    void Awake()
    {
        //since the camera will be the children of the player, we can get the camera component by getting the first children of the player
        playerCamera = GetComponentInChildren<Camera>();
        //get the character controller component
        characterController = GetComponent<CharacterController>();
        //lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //for hiding the cursor


    }

    // Update is called once per frame
    void Update()
    {
        if(CanMove)
        {
            HandleMovementInput();
            HandleLookInput();

            ApplyFinalMovement();

        }
    }

    //the function is our keyboard controller
    private void HandleMovementInput()
    {
        //get the input from the keyboard
        currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    private void HandleLookInput()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void ApplyFinalMovement()
    {
        if(!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }   
}
