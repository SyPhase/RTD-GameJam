using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void OnDisable()
    {
        // Tell GameManager that Player is out
        _gameManager.PlayerDied();
    }
}