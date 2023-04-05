using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject playerHead;
    public GameObject groundCheck;
    public AudioSource footsteps;
    public Animator gunAnimator;
    public float moveSpeed, lookSensitivity, maxForce, jumpForce;
    private float verticalVelocity;
    public Vector2 velocity, look;
    public bool grounded;
    private float lookRotation;
    public float gravity;
    private float lookAngle;

    public void OnMove(InputAction.CallbackContext context)
    {
        velocity = context.ReadValue<Vector2>();
        if (velocity.magnitude == 0)
        {
            gunAnimator.SetBool("Moving", false);
            footsteps.enabled = false;
        }

        else
        {
            gunAnimator.SetBool("Moving", true);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump");
        Jump();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        verticalVelocity = gravity * Time.deltaTime;
       
        // Find target velcocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(velocity.x, 0, velocity.y);
        targetVelocity *= moveSpeed;

        // Allign direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Calculate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, verticalVelocity, velocityChange.z);

        // Clamp force
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (grounded && velocity.magnitude != 0)
        {
            footsteps.enabled = true;

        }

        else
            footsteps.enabled = false;
    }

    void Jump()
    {
        Vector3 jumpingForces = Vector3.zero;
        if (grounded)
        {
            jumpingForces = Vector3.up * jumpForce;
        }

        rb.AddForce(jumpingForces, ForceMode.VelocityChange);
    }

    void Look()
    {
        //Turn player on 1 axis
        transform.Rotate(Vector3.up * look.x * lookSensitivity);

        //Look up and down
        lookRotation += (-look.y * lookSensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -60, 50);
        playerHead.transform.eulerAngles = new Vector3(lookRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Look();
    }

    public void SetGrounded(bool state)
    {
        grounded = state;
    }
}
