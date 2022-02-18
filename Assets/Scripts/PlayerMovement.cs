using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Vector3 velocity;
    private float defaultScaleY;
    private float defaultPositionY;

    [Header("Movement properties")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeedMult = 2f;
    [SerializeField] private float crouchSpeedMult = 0.5f;
    [SerializeField] private float crouchScaleYMult = 0.5f;
    [SerializeField] private float crouchTransitionSpeed = 1f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravityForce = -9.81f;
    [SerializeField] public float defaultVelocityY = -1f;

    [Header("Environment properties")]
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingCheckRadius = 0.25f;
    [SerializeField] private LayerMask ceilingMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundMask;

    public bool isRunning = false;
    private bool isCrouching = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultScaleY = transform.localScale.y;
        defaultPositionY = transform.position.y;
    }

    private void Update()
    {
        HandleMovement();
        HandleVelocity();

        if (IsOnGround())
        {
            if (Input.GetButton("Sprint"))
            {
                //SwitchCrouching(!isCrouching);
            }
            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = !isCrouching;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleCrouching();
    }

    private void HandleMovement()
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirY = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * dirX + transform.forward * dirY;

        if (IsOnGround())
        {
            if (Input.GetButton("Sprint"))
            {
                direction *= sprintSpeedMult;
                isCrouching = false;
            }
            else if (isCrouching)
            {
                direction *= crouchSpeedMult;
            }
        }

        controller.Move(direction * speed * Time.deltaTime);
    }

    //private void SwitchCrouching(bool newState)
    //{
    //    isCrouching = newState;
    //float newHeight = defaultHeight;
    //float NewPositionY = defaultPositionY;

    //if (isCrouching)
    //{
    //    newHeight /= 2f;
    //    NewPositionY = crouchPositionY;
    //}

    //transform.localScale = new Vector3(transform.localScale.x, newHeight, transform.localScale.z);
    //transform.position = new Vector3(transform.position.x, NewPositionY, transform.position.z);
    //}

    private void HandleVelocity()
    {
        if (IsOnGround() || IsUnderCeiling() && velocity.y > defaultVelocityY)
        {
            velocity.y = defaultVelocityY;
        }
        else
        {
            velocity.y += gravityForce * Time.deltaTime;
            if (isCrouching && (velocity.y < defaultVelocityY - 1f || velocity.y > defaultVelocityY + 1f))
            {
                isCrouching = false;
            }
        }

        if (IsOnGround() && Input.GetButton("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravityForce);
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCrouching()
    {
        float newScaleY = transform.localScale.y;
        float targetScaleY = defaultScaleY;
        float newPositionY = transform.position.y;
        float targetPositionY = newPositionY + 0.5f * crouchScaleYMult;

        if (isCrouching)
        {
            targetScaleY *= crouchScaleYMult;
            targetPositionY = newPositionY - 2f * crouchScaleYMult;
        }

        if (newScaleY < targetScaleY - 0.05f || newScaleY > targetScaleY + 0.05f)
        {
            newScaleY = Mathf.Lerp(newScaleY, targetScaleY, Time.deltaTime * crouchTransitionSpeed);
            newPositionY = Mathf.Lerp(newPositionY, targetPositionY, Time.deltaTime * crouchTransitionSpeed);
            transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, newPositionY, transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, targetScaleY, transform.localScale.z);
        }
    }

    private bool IsUnderCeiling()
    {
        return Physics.CheckSphere(ceilingCheck.position, ceilingCheckRadius, ceilingMask);
    }

    private bool IsOnGround()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }
}
