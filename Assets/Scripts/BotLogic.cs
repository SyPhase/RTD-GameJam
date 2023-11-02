using UnityEngine;

public class BotLogic : MonoBehaviour
{
    [Range(-1f, 1f)]
    [SerializeField] float _targetDirection = 0f;

    [Range(1.6f, 9.75f)]
    [SerializeField] float _targetHeight = 2.5f;

    Bot _bot;
    float _nextFlapTime = 0f;
    float _nextFlapHeight = 0f;

    void Start()
    {
        _bot = GetComponent<Bot>();
    }

    void FixedUpdate()
    {
        // Handle Horizontal movement
        _bot.ChangeMovementValue(_targetDirection);

        // Handle flapping to try and reach a target height
        if ((_nextFlapTime <= Time.fixedTime) && (transform.position.y <= _nextFlapHeight))
        {
            _bot.Flap();

            _nextFlapHeight = _targetHeight + Random.Range(-0.5f, 0.5f);
            _nextFlapTime = Time.fixedTime + (5 * Time.fixedDeltaTime);
        }
    }

    // Set between -1 and 1 (-1 means left, 1 means right)
    public void SetTargetDirection(float targetDirection)
    {
        if (targetDirection > 1)
        {
            _targetDirection = 1;
        }
        else if (targetDirection < -1)
        {
            _targetDirection = -1;
        }
        else
        {
            _targetDirection = targetDirection;
        }
    }

    // Set between 1.6 and 9.75 (1.6 is lowest, 9.75 is highest)
    public void SetTargetHeight(float targetHeight)
    {
        if (targetHeight > 9.75f)
        {
            _targetHeight = 9.75f;
        }
        else if (targetHeight < 1.6f)
        {
            _targetHeight = 1.6f;
        }
        else
        {
            _targetHeight = targetHeight;
        }
    }
}