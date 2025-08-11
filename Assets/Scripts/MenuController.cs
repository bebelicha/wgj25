using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToHomeScreen()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToMainMenu();
        }
    }
    public void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
    }
    public void ToggleLanguage()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.ToggleLanguage();
        }
    }
}