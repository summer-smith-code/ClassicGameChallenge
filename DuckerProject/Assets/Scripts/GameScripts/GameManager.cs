using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Ensure only one GameManager exists
    public static GameManager Instance;

    // Total number of lives and coves
    public int totalLives = 3;
    public int totalCoves = 5;

    // Current values that track player's lives and filled coves
    [HideInInspector] public int currentLives;
    [HideInInspector] public int covesFilled;

    // Prevent multiple life loss calls during a single death event
    private bool isPlayerDead = false;

    // Reference to the TMP
    public TextMeshProUGUI livesText;

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

        // Log the initial game state
        Debug.Log("Game started with " + totalLives + " lives and " + totalCoves + " coves.");

        // Update the UI
        UpdateLivesText();
    }

    // Method called when the player loses a life
    public void PlayerLostLife()
    {
        // Prevent double life loss
        if (isPlayerDead) return;

        // Mark the player as dead
        isPlayerDead = true;

        // Decrease the player's remaining lives
        currentLives--;

        // Log the remaining lives
        Debug.Log("Player lost a life. Lives remaining: " + currentLives);

        // Update the UI
        UpdateLivesText();

        // Check if the player has run out of lives
        if (currentLives <= 0)
        {
            // Game Over message
            Debug.Log("Game Over!");
            GameOver();
        }
        else
        {
            // Reset the death flag after a short delay
            Invoke("ResetDeathFlag", 1f);
        }
    }

    // Reset the isPlayerDead flag
    private void ResetDeathFlag()
    {
 
        isPlayerDead = false;
    }


    public void CoveFilled()
    {
        // Fill 1 cove
        covesFilled++;

        // Log the number of filled coves
        Debug.Log("Cove filled! Total coves filled: " + covesFilled);

        // Check if the player has filled all the coves
        if (covesFilled >= totalCoves)
        {
            // Victory message
            Debug.Log("You Win!");
            WinGame();
        }
    }

    // Update the TMP
    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
        else
        {
            Debug.LogWarning("LivesText UI element is not assigned!");
        }
    }

    private void GameOver()
    {
        // Handle game over logic
        Debug.Log("Game Over!");
        //Link to lose screen
    }

    private void WinGame()
    {
        // Handle win logic
        Debug.Log("You won! Victory!");
        //Link to win screen
    }
}
