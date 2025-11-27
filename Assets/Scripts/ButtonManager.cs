using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void playNextLevel()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
