using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    BotManager _botManager;

    void Start()
    {
        _botManager = FindObjectOfType<BotManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.position.y < collision.transform.position.y)
            {
                // Player/Bot dies
                gameObject.SetActive(false);
                _botManager.TryStartNewWave();
            }
        }

        if (collision.gameObject.CompareTag("Kill"))
        {
            // Player/Bot dies
            gameObject.SetActive(false);
            _botManager.TryStartNewWave();
        }
    }
}