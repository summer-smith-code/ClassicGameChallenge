using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Pressed Play");
        if (GameManager.Instance != null)
            GameManager.Instance.ResetGame();
        SceneManager.LoadScene(1);
    }

    public void Play2()
    {
        Debug.Log("Pressed Play2");
        if (GameManager.Instance != null)
            GameManager.Instance.ResetGame();
        SceneManager.LoadScene(2);
    }

    public void Menu()
    {
        Debug.Log("Pressed Menu");
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Debug.Log("Pressed Quit");
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }
}
