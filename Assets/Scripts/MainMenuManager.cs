using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public string firstLevel;

    public void LoadFirstLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(firstLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
