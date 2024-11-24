using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;

    // Các biến vận tốc
    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    // Tham số di chuyển
    public float acceleration = 2.0f;
    public float decceleration = 2.0f;

    // Tốc độ tối đa
    public float maximumRunVelocity = 0.5f;
    public float maximumSprintVelocity = 1.0f;
    public float maximumSprintAltVelocity = 2.0f;
    public float jumpForce = 1.0f;

    // Trạng thái
    private bool hasDoublePressedW = false;
    private bool isSprinting = false;
    private bool isSprintAlt = false;
    private bool isJumping = false;
    private bool isGrounded = true;
    private bool isFalling = false;
    private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleJump();
        UpdateAnimator();
    }

    private void HandleInput()
    {
        // Nhận các đầu vào
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (hasDoublePressedW)
            {
                isSprinting = true;
            }
            else
            {
                hasDoublePressedW = true;
                Invoke("ResetDoubleW", 0.5f);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && isSprinting)
        {
            isSprintAlt = !isSprintAlt;
        }
    }

    private void HandleMovement()
    {
        // Tính toán tốc độ tối đa dựa trên trạng thái
        float maxVelocity = isSprintAlt ? maximumSprintAltVelocity : isSprinting ? maximumSprintVelocity : maximumRunVelocity;

        // Di chuyển
        bool forwardPress = Input.GetKey("w");
        bool backPress = Input.GetKey("s");
        bool leftPress = Input.GetKey("a");
        bool rightPress = Input.GetKey("d");

        UpdateVelocity(forwardPress, backPress, leftPress, rightPress, maxVelocity);
    }

    private void UpdateVelocity(bool forwardPress, bool backPress, bool leftPress, bool rightPress, float maxVelocity)
    {
        // Di chuyển theo chiều Z
        if (forwardPress && velocityZ < maxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
            isMoving = true;
        }
        else if (backPress && velocityZ > -maximumRunVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
            isMoving = true;
        }
        else
        {
            velocityZ = Mathf.MoveTowards(velocityZ, 0, Time.deltaTime * decceleration);
            if (!backPress && !forwardPress) isMoving = false;
        }

        // Di chuyển theo chiều X
        if (leftPress && velocityX > -maximumRunVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
            isMoving = true;
        }
        else if (rightPress && velocityX < maximumRunVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
            isMoving = true;
        }
        else
        {
            velocityX = Mathf.MoveTowards(velocityX, 0, Time.deltaTime * decceleration);
            if (velocityZ == 0 && velocityX == 0) isMoving = false;
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            isGrounded = false;
            animator.SetBool("isJump", true);
            isMoving = true;
        }

        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            if (isJumping && isGrounded)
            {
                isJumping = false;
                animator.SetBool("isJump", false);
            }

            if (isFalling && isGrounded)
            {
                isFalling = false;
            }
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("Velocity Z", velocityZ);
        animator.SetFloat("Velocity X", velocityX);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isMoving", isMoving);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded"))
        {
            isGrounded = true;
            isFalling = false;

            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("isJump", false);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded"))
        {
            isGrounded = false;
        }
    }

    private void ResetDoubleW()
    {
        hasDoublePressedW = false;
    }
}
