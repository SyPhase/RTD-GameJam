using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotLogic : MonoBehaviour
{
    [Range(-1f, 1f)]
    [SerializeField] float _targetDirection = 0f;

    [Range(1.6f, 9.75f)]
    [SerializeField] float _targetHeight = 2.5f;

    Bot _bot;
    float _nextFlapTime = 0f;

    void Start()
    {
        _bot = GetComponent<Bot>();
    }

    void FixedUpdate()
    {
        // Handle Horizontal movement
        _bot.ChangeMovementValue(_targetDirection);

        float heightVariance = Random.Range(-0.5f, 0.5f);

        // Handle flapping to try and reach a target height
        if ((_nextFlapTime <= Time.fixedTime) && (transform.position.y <= _targetHeight))
        {
            _bot.Flap();

            _nextFlapTime = Time.fixedTime + (5 * Time.fixedDeltaTime);
        }
    }

    public void SetTargetHeight(float targetHeight)
    {
        _targetHeight = targetHeight;
    }
}