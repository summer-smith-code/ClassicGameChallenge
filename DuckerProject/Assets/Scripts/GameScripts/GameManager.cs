using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ensure only one GameManager exists
    public static GameManager Instance;

    // Total number of lives and coves (not functional yet)
    public int totalLives = 3;
    public int totalCoves = 5;

    // Current values that track player's lives and filled coves
    [HideInInspector] public int currentLives;
    [HideInInspector] public int covesFilled;

    //prevent multiple life loss calls during a single death event
    private bool isPlayerDead = false;


    private void Awake()
    {
        // Check if an instance already exists
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
        //reset variables
        currentLives = totalLives;
        covesFilled = 0;

        // Log the initial game state
        Debug.Log("Game started with " + totalLives + " lives and " + totalCoves + " coves.");
    }

    // Method called when the player loses a life
    public void PlayerLostLife()
    {
         // Prevent double life loss
        if (isPlayerDead) return;

        // Mark the player as dead for the current death event
        isPlayerDead = true;

        // Decrease the player's remaining lives
        currentLives--;

        // Log the remaining lives after the player loses a life
        Debug.Log("Player lost a life. Lives remaining: " + currentLives);

        // Check if the player has run out of lives
        if (currentLives <= 0)
        {
            // Game Over message
            Debug.Log("Game Over!");
  
            // GameOver(); (not ready)
        }

    }

    // Reset the isPlayerDead flag
    public void ResetDeathFlag()
    {
        // Reset flag to allow life loss again on the next death event
        isPlayerDead = false;
    }

    // Method called when a cove is filled
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

            // WinGame(); (not ready)
        }
    }


    private void GameOver()
    {
        // Stop the game or trigger level restart
        Debug.Log("Game Over! Restarting level...");
        //link to Ui loss
    }


    private void WinGame()
    {
        // Stop the game or trigger victory screen
        Debug.Log("You won! Victory!");
        //link to ui win

    }
}
