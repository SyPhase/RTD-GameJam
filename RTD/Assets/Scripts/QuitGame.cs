using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuiteGame() //InputAction.CallbackContext context
    {
        Application.Quit();
        Debug.Log("ERROR: User tried to quit game");
    }
}