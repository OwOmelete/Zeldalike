using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TopDownPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float rotationSpeed = 15f;
    public float inputResponsiveness = 1f;
    
    [Header("Slope Settings")]
    public float gravity = 30f;
    public float stickToGroundForce = 5f;
    public float slopeForce = 8f;

    [Header("Dash Settings")] 
    public float dashDuration;
    public float dashSpeed;
    public float dashCooldown;

    [Header("References")]
    public Transform cameraTransform;

    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private Vector3 currentHorizontalVelocity;

    private float dashTimer = 0f;
    private Vector3 dashDirection;
    private float lastDash;

    private float lastX;
    private float lastY;
    
    private bool isDashing = false;

    private void OnEnable()
    {
        DeathZone.OnFall += Fall;
    }

    private void OnDisable()
    {
        DeathZone.OnFall -= Fall;
    }

    private void Fall(Transform t)
    {
        controller.enabled = false;
        transform.position = t.position;
        controller.enabled = true;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
    }

    private void Dash()
    {
        if (isDashing || Time.time - lastDash < dashCooldown) return;

        isDashing = true;
        dashTimer = dashDuration;
        lastDash = Time.time;
        animator.SetTrigger("Dash");

        dashDirection = moveDirection.normalized;

        if (dashDirection.magnitude < 0.1f)
        {
            dashDirection = transform.forward;
        }
    }
    
    private void OnRightTrigger()
    {
        Dash();
    }
    
    void HandleMovement()
    {
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
        
        
        
        

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        
        
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDirection = (camForward * input.z + camRight * input.x).normalized;


        if (input.magnitude > 0.2f)
        {
            animator.SetBool("IsWalking", true);
            lastX = -horizontal;
            lastY = -vertical;
            animator.SetFloat("AxesX", lastX);
            animator.SetFloat("AxesY", lastY);
        }
        else
        {
            
            animator.SetBool("IsWalking", false);
            
            animator.SetFloat("AxesX", lastX);
            animator.SetFloat("AxesY", lastY);
        }
        

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            if (dashTimer <= 0f)
                isDashing = false;

            return;
        }
        
        if (targetDirection.magnitude > 0.1f)
        {
            moveDirection = Vector3.Lerp(
                moveDirection,
                targetDirection,
                inputResponsiveness * Time.deltaTime * 10f
            );

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        Vector3 targetVelocity = moveDirection * moveSpeed * input.magnitude;

        float accel = input.magnitude > 0.1f ? acceleration : deceleration;

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            accel * Time.deltaTime
        );

        if (controller.isGrounded)
        {
            velocity.y = -stickToGroundForce;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        Vector3 finalVelocity = currentHorizontalVelocity;
        finalVelocity.y = velocity.y;

        controller.Move(finalVelocity * Time.deltaTime);
    }
}