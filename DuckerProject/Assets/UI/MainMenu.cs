using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class MainMenu : MonoBehaviour
{
    public void Menu()
    {
        Debug.Log("Pressed Menu");
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        Debug.Log("Pressed Play");
        SceneManager.LoadScene(3);
    }

    public void Play2()
    {
        Debug.Log("Pressed Play2");
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Debug.Log("Pressed Quit");
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
}
