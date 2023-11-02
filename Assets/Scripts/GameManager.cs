using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    InputActionMap _playerActionMap;

    void Awake()
    {
        _playerActionMap = FindObjectOfType<PlayerInput>().actions.FindActionMap("Player");
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
}