using UnityEngine;

public class GameManager : MonoBehaviour
{
    //ensure only one GameManager exists
    public static GameManager Instance;

    // Total number of lives and coves (not functional yer)
    public int totalLives = 3;
    public int totalCoves = 5;

    // Current values that track player's lives and filled coves
    [HideInInspector] public int currentLives;
    [HideInInspector] public int covesFilled;

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Check if an instance already exists. If so, destroy the duplicate.
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
        // Set the currentLives to totalLives and set covesFilled to 0
        currentLives = totalLives;
        covesFilled = 0;

        // Log the initial game state
        Debug.Log("Game started with " + totalLives + " lives and " + totalCoves + " coves.");
    }

    // Method called when the player loses a life (not functional yet)
    public void PlayerLostLife()
    {
        // Decrease the player's remaining lives
        currentLives--;
        // Log the remaining lives after the player loses a life
        Debug.Log("Player lost a life. Lives remaining: " + currentLives);

        // Check if the player has run out of lives
        if (currentLives <= 0)
            Debug.Log("Game Over!");  // Game Over message when lives reach 0
    }

    // Method called when a cove is filled
    public void CoveFilled()
    {
        // Increment the number of filled coves
        covesFilled++;
        // Log the number of filled coves
        Debug.Log("Cove filled! Total coves filled: " + covesFilled);

        // Check if the player has filled all the coves
        if (covesFilled >= totalCoves)
            Debug.Log("You Win!");  // Victory message when all coves are filled
    }
}
