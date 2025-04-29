using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function can be linked to your button's OnClick event via the Inspector
    public void LoadGameplay()
    {
        // Replace "GameplayScene" with the exact name of your gameplay scene
        SceneManager.LoadScene("GameplayScene");
    }

    // Optionally, for an Exit button
    public void ExitGame()
    {
        // This only works in a built application, does nothing in the Unity Editor
        Application.Quit();
    }
}

