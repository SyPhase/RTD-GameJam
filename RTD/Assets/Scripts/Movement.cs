using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Player movement forces & max forces")]
    [Tooltip("Upward force when flapping - Default: 150")]
    [SerializeField] float flapForce = 150f;
    [Tooltip("Vertical velocity maximum (flapping and falling) - Default: 4")]
    [SerializeField] float flapMaxSpeed = 4f;

    [Tooltip("Horizontal force increase when flapping directionally - Default: 50")]
    [SerializeField] float flapHorizontalControl = 50f;

    [Tooltip("Horizontal force increase when moving - Default: 10")]
    [SerializeField] float moveForce = 10f;
    [Tooltip("Horizontal velocity maximum (movement side to side) - Default: 10")]
    [SerializeField] float moveMaxSpeed = 10f;

    [Tooltip("Horizontal velocity minimum before character will stop moving - Default: 0.5")]
    [SerializeField] float minVelStop = 0.5f;

    [Header("SFXs")]
    [Tooltip("Sound file for 'Sliding' when slowing down on the ground")]
    [SerializeField] AudioClip slideSFX;
    [Tooltip("Sound file for 'Flapping' when player flap input")]
    [SerializeField] AudioClip flapSFX;

    AudioSource audioSource;

    Rigidbody rb;
    Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate() // for Physics updates
    {
        rb.AddRelativeForce(moveInput.x * moveForce, 0f, 0f); // adds horizontal force for movement

        SpeedControl();

        ManageSound();
    }

    void ManageSound()
    {
        // Plays sound only if not moving up or down
        if (Mathf.Approximately(rb.velocity.y, 0.0f))
        {
            // Plays sound only if input direction is opposite of velocity direction
            if((moveInput.x > 0 && rb.velocity.x < 0) || (moveInput.x < 0 && rb.velocity.x > 0))
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(slideSFX); // plays slideSFX
                }
            }
            //else
            //{
            //    audioSource.Stop();
            //}
        }
        //else
        //{
        //    audioSource.Stop();
        //}
    }

    public void PlayerMove(InputAction.CallbackContext context) // Called on player input : Move
    {
        moveInput = context.ReadValue<Vector2>(); // sets value to the player input
    }

    public void PlayerFlap(InputAction.CallbackContext context) // Called throught Unity Events when Flap (Spacebar)
    {
        if (!context.performed) { return; } // Stops single button press from triggering 3 times...

        rb.AddRelativeForce(0, flapForce, 0, ForceMode.Force); // Adds upward force, set in inspector

        // Air control: flap affects horizontal velocity if player is moving
        if (moveInput.x != 0)
        {
            rb.AddRelativeForce(moveInput.x * flapHorizontalControl, 0f, 0f);
        }

        // Plays flapSFX after stopping other sounds
        audioSource.Stop();
        audioSource.PlayOneShot(flapSFX); // plays flapSFX
    }

    void SpeedControl() // limits velocity to max values (set in inspector)
    {
        // limit vertical velocity if more than flapMaxSpeed
        Vector3 flatVelocity = new Vector3(0f, rb.velocity.y, 0f);
        if (flatVelocity.magnitude > flapMaxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * flapMaxSpeed;
            rb.velocity = new Vector3(rb.velocity.x, limitedVelocity.y, 0f);
        }

        // limit horizontal velocity if more than moveMaxSpeed
        flatVelocity = new Vector3(rb.velocity.x, 0f, 0f);
        if (flatVelocity.magnitude > moveMaxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveMaxSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, 0f);
        }

        // stops player if not inputing movement & has velocity below minVelStop
        if (moveInput.x == 0 && flatVelocity.magnitude < minVelStop)
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }
}