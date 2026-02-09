using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Ensure only one GameManager exists
    public static GameManager Instance;

    // Total number of lives and coves
    public int totalLives = 3;
    public int totalCoves = 10;

    // Current values that track player's lives and filled coves
    [HideInInspector] public int currentLives;
    [HideInInspector] public int covesFilled;

    // Prevent multiple life loss calls during a single death event
    private bool isPlayerDead = false;

    [Header("Lives UI")]
    public Image[] lifeImages;
    public Sprite fullLifeSprite;
    public Sprite emptyLifeSprite;

    private void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // Reset variables
        currentLives = totalLives;
        covesFilled = 0;

        Debug.Log("Game started with " + totalLives + " lives and " + totalCoves + " coves.");

        // Update the UI
        UpdateLivesUI();
    }

    // Method called when the player loses a life
    public void PlayerLostLife()
    {
        if (isPlayerDead) return;

        isPlayerDead = true;

        currentLives--;
        currentLives = Mathf.Max(currentLives, 0);

        Debug.Log("Player lost a life. Lives remaining: " + currentLives);

        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(ResetDeathFlag), 1f);
        }
    }

    private void ResetDeathFlag()
    {
        isPlayerDead = false;
    }

    public void CoveFilled()
    {
        covesFilled++;

        Debug.Log("Cove filled! Total coves filled: " + covesFilled);

        if (covesFilled >= totalCoves)
        {
            WinGame();
        }
    }

    //Life UI
    private void UpdateLivesUI()
    {
        if (lifeImages == null || lifeImages.Length == 0)
        {
            Debug.LogWarning("Life Images not assigned!");
            return;
        }

        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (lifeImages[i] == null) continue;

            if (i < currentLives)
            {
                lifeImages[i].sprite = fullLifeSprite;
            }
            else
            {
                lifeImages[i].sprite = emptyLifeSprite;
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("LoseScreen");
    }

    private void WinGame()
    {
        Debug.Log("You won!");
        SceneManager.LoadScene("WinScreen");
    }
}
