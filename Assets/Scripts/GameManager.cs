using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    InputActionMap _playerActionMap;

    void Awake()
    {
        _playerActionMap = FindObjectOfType<PlayerInput>().actions.FindActionMap("Player");

        Cursor.visible = false;
    }

    void OnEnable()
    {
        _playerActionMap.FindAction("Quit").started += HandleQuit;
    }

    void OnDisable()
    {
        _playerActionMap.FindAction("Quit").started -= HandleQuit;
    }

    void HandleQuit(InputAction.CallbackContext obj)
    {
        QuitGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("ERROR: User tried to quit game");
    }

    public void PlayerDied()
    {
        PlayerInput[] players = FindObjectsOfType<PlayerInput>();
        if (players.Length <= 0)
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(0);
        print("STATUS: Game restarted");
    }
}