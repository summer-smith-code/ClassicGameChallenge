using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class MainMenu : MonoBehaviour
{
    public Scene firstLevel;

    public void Play()
    {
        SceneManager.LoadScene("firstLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
