using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Tooltip("Upward force when flapping")]
    [SerializeField] float flapForce = 150f;
    [Tooltip("Vertical velocity maximum (flapping and falling)")]
    [SerializeField] float flapMaxSpeed = 4f;

    [Tooltip("Horizontal force increase when moving")]
    [SerializeField] float moveForce = 10f;
    [Tooltip("Horizontal velocity maximum (movement side to side)")]
    [SerializeField] float moveMaxSpeed = 10f;

    Rigidbody rb;
    Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // for Physics updates
    {
        rb.AddRelativeForce(moveInput.x * moveForce, 0f, 0f); // adds horizontal force for movement

        SpeedControl();
    }

    public void PlayerMove(InputAction.CallbackContext context) // Called on player input : Move
    {
        moveInput = context.ReadValue<Vector2>(); // sets value to the player input
        //Debug.Log("Player Input : " + context.ReadValue<Vector2>());
    }

    public void PlayerFlap(InputAction.CallbackContext context) // Called throught Unity Events when Flap (Spacebar)
    {
        if (!context.performed) { return; } // Stops single button press from triggering 3 times...

        rb.AddRelativeForce(0, flapForce, 0, ForceMode.Force); // Adds upward force, set in inspector
        //Debug.Log("Player Input : Flap");
    }

    void SpeedControl() // stops vertical velocity from exceeding flapMaxSpeed (set in inspector)
    {
        Vector3 flatVelocity = new Vector3(0f, rb.velocity.y, 0f);

        // limit vertical velocity if more than movementSpeed
        if (flatVelocity.magnitude > flapMaxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * flapMaxSpeed;
            rb.velocity = new Vector3(rb.velocity.x, limitedVelocity.y, 0f);
        }

        flatVelocity = new Vector3(rb.velocity.x, 0f, 0f);

        if (flatVelocity.magnitude > moveMaxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveMaxSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, 0f);
        }
    }
}