using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bot : MonoBehaviour
{
    [Header("Bot movement forces & max forces")]
    [Tooltip("Upward force when flapping - Default: 150")]
    [SerializeField] float _flapForce = 150f;
    [Tooltip("Horizontal force increase when moving - Default: 10")]
    [SerializeField] float _horizontalMoveForce = 10f;

    [Tooltip("Vertical velocity maximum (flapping and falling) - Default: 5")]
    [SerializeField] float _maxFlapSpeed = 5f;
    [Tooltip("Horizontal velocity maximum (movement side to side) - Default: 10")]
    [SerializeField] float _maxHorizontalSpeed = 10f;

    [Tooltip("Horizontal force increase when flapping directionally - Default: 50")]
    [SerializeField] float _horizontalFlapControl = 50f;

    [Tooltip("Horizontal velocity minimum before character will stop moving - Default: 0.5")]
    [SerializeField] float _minStopVelocity = 0.5f;

    Rigidbody _rigidbody;
    float _movementValue = 0;

    Vector3 _lastPosition;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If stuck
        if (_lastPosition == transform.position)
        {
            Flap();
        }

        // Cache last position
        _lastPosition = transform.position;

        // Horizontal movement
        _rigidbody.AddRelativeForce(_movementValue * _horizontalMoveForce * _rigidbody.mass, 0f, 0f); // adds horizontal force for movement

        SpeedControl();
    }

    // Copied from Player's Movement.cs
    void SpeedControl() // limits velocity to max values (set in inspector)
    {
        // limit vertical velocity if more than flapMaxSpeed
        Vector3 flatVelocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
        if (flatVelocity.magnitude > _maxFlapSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxFlapSpeed;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, limitedVelocity.y, 0f);
        }

        // limit horizontal velocity if more than moveMaxSpeed
        flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, 0f);
        if (flatVelocity.magnitude > _maxHorizontalSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _maxHorizontalSpeed;
            _rigidbody.velocity = new Vector3(limitedVelocity.x, _rigidbody.velocity.y, 0f);
        }

        // stops player if not inputing movement & has velocity below minVelStop
        if (_movementValue == 0 && flatVelocity.magnitude < _minStopVelocity)
        {
            _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
        }
    }

    // Makes the bot Flap
    public void Flap()
    {
        _rigidbody.AddRelativeForce(0, _flapForce * _rigidbody.mass, 0); // Adds upward force, set in inspector

        // Air control: flap affects horizontal velocity if player is moving
        if (_movementValue != 0)
        {
            _rigidbody.AddRelativeForce(_movementValue * _horizontalFlapControl * _rigidbody.mass, 0f, 0f);
        }
    }

    // Set a value between -1 and 1 (left to right)
    public void ChangeMovementValue(float movementValue)
    {
        _movementValue = movementValue;
    }
}