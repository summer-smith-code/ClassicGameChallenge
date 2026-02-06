using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    Image[] health;

    [SerializeField]
    Sprite full;
    [SerializeField]
    Sprite empty;

    //Put reference to the game manager here
    public GameManager gameManager;

    private void Update()
    {
        int count = 0;
        foreach (Image i in health)
        {
            if (gameManager.currentLives > count)
                i.sprite = full;
            else
                i.sprite = empty;
            count++;
        }
    }
}
