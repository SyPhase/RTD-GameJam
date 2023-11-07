using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Player movement forces & max forces")]
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

    [Header("SFXs")]
    [Tooltip("Sound file for 'Sliding' when slowing down on the ground")]
    [SerializeField] AudioClip _slideSFX;
    [Tooltip("Sound file for 'Flapping' when player flap input")]
    [SerializeField] AudioClip _flapSFX;

    AudioSource _playerAudioSource;

    Rigidbody _rigidbody;
    float _movementValue;

    bool _isGrounded = false;

    // Input
    PlayerInput _playerInput;
    InputActionAsset _inputActionAsset;
    InputActionMap _playerActionMap;
    InputAction _movementAction;

    void Awake()
    {
        // Input
        _playerInput = GetComponent<PlayerInput>();
        _inputActionAsset = _playerInput.actions;
        _playerActionMap = _inputActionAsset.FindActionMap("Player");
    }

    void OnEnable()
    {
        // Input
        _movementAction = _playerActionMap.FindAction("Movement");
        _playerActionMap.FindAction("Flap").started += HandleFlap;
        _playerActionMap.Enable();
    }

    void OnDisable()
    {
        // Input
        _playerActionMap.FindAction("Flap").started -= HandleFlap;
        _playerActionMap.Disable();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerAudioSource = GetComponent<AudioSource>();
    }

    void HandleFlap(InputAction.CallbackContext obj)
    {
        _rigidbody.AddRelativeForce(0, _flapForce * _rigidbody.mass, 0); // Adds upward force, set in inspector

        // Air control: flap affects horizontal velocity if player is moving
        if (_movementValue != 0)
        {
            _rigidbody.AddRelativeForce(_movementValue * _horizontalFlapControl * _rigidbody.mass, 0f, 0f);
        }

        // Plays flapSFX after stopping other sounds
        _playerAudioSource.Stop();
        _playerAudioSource.PlayOneShot(_flapSFX); // plays flapSFX
    }

    void OnTriggerEnter(Collider other)
    {
        _isGrounded = true;
    }

    void OnTriggerExit(Collider other)
    {
        _isGrounded = false;
    }

    void FixedUpdate() // for Physics updates
    {
        // Cache movement action values
        _movementValue = _movementAction.ReadValue<Vector2>().x;

        // Horizontal movement
        _rigidbody.AddRelativeForce(_movementValue * _horizontalMoveForce * _rigidbody.mass, 0f, 0f); // adds horizontal force for movement

        SpeedControl();

        ManageSound();
    }

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

    void ManageSound()
    {
        // Plays sound only if not moving up or down
        if (_isGrounded && _rigidbody.velocity.y < 0.01f)
        {
            // Plays sound only if input direction is opposite of velocity direction
            if((_movementValue > 0 && _rigidbody.velocity.x < -3) || (_movementValue < 0 && _rigidbody.velocity.x > 3))
            {
                if (!_playerAudioSource.isPlaying)
                {
                    _playerAudioSource.PlayOneShot(_slideSFX, 0.3f); // plays slideSFX
                }
            }
        }
    }
}