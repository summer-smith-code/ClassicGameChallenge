using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int totalLives = 3;
    public int totalCoves = 10;

    [HideInInspector] public int currentLives;
    [HideInInspector] public int covesFilled;
    public int points = 0;

    private bool isPlayerDead = false;

    [Header("Heart UI")]
    public Sprite fullLifeSprite;
    public Sprite emptyLifeSprite;
    private Image[] lifeImages;

    [Header("Score UI")]
    public TMP_Text scoreText;

    [Header("Scoring Settings")]
    public int pointsPerHop = 10;

    //Ensures only 1 game manager
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //reset game state
    private void Start()
    {
        if (currentLives == 0)
            ResetGame();
    }

    //resets game state when replaying from menu or loading a new scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only update UI if this is a level scene, not Win/Lose screen
        if (scene.name != "WinScreen" && scene.name != "LoseScreen")
        {
            ConnectLifeUI();
            ConnectScoreUI();
            UpdateLivesUI();
            UpdateScoreUI();
        }
    }

    public void ResetGame()
    {
        currentLives = totalLives;
        covesFilled = 0;
        points = 0;
        isPlayerDead = false;

        UpdateLivesUI();
        UpdateScoreUI();
        Debug.Log("Game reset: Lives=" + currentLives + ", Points=" + points + ", Coves=" + covesFilled);
    }

    //connects UI when playing from menu
    private void ConnectLifeUI()
    {
        Image[] allImages = FindObjectsOfType<Image>();
        lifeImages = System.Array.FindAll(allImages, img => img.name.ToLower().StartsWith("life"));
        System.Array.Sort(lifeImages, (a, b) => a.name.CompareTo(b.name));

        if (lifeImages.Length == 0)
            Debug.LogWarning("No life images found! Make sure your hearts are named life1, life2, life3.");
    }

    private void ConnectScoreUI()
    {
        if (scoreText == null)
        {
            TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>();
            foreach (TMP_Text tmp in allTMPTexts)
            {
                if (tmp.name == "ScoreText")
                {
                    scoreText = tmp;
                    break;
                }
            }

            if (scoreText == null)
                Debug.LogWarning("No TMP_Text named 'ScoreText' found! Add a TextMeshPro UI element and name it 'ScoreText'.");
        }
    }

    //called when player loses a life
    public void PlayerLostLife()
    {
        if (isPlayerDead) return;

        isPlayerDead = true;
        currentLives = Mathf.Max(currentLives - 1, 0);
        Debug.Log("Player lost a life. Lives remaining: " + currentLives);

        UpdateLivesUI();

        if (currentLives <= 0)
            GameOver();
        else
            Invoke(nameof(ResetDeathFlag), 0.1f);
    }

    private void ResetDeathFlag() => isPlayerDead = false;

    //called when player reaches a cove
    public void CoveFilled()
    {
        covesFilled++;
        points += 50;
        UpdateScoreUI();
        Debug.Log("Cove filled! Total coves: " + covesFilled);

        if (covesFilled >= totalCoves)
        {
            points += currentLives * 200;
            UpdateScoreUI();
            WinGame();
        }
    }

    //tracks hop score
    public void PlayerHopped()
    {
        points += pointsPerHop;
        UpdateScoreUI();
        Debug.Log("Player hopped! Points: " + points);
    }

    private void UpdateLivesUI()
    {
        if (lifeImages == null || lifeImages.Length == 0) return;

        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (lifeImages[i] == null) continue;
            lifeImages[i].sprite = (i < currentLives) ? fullLifeSprite : emptyLifeSprite;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + points;
        }
    }

    // ---- UPDATED WIN/LOSE HANDLING ---- //

    private void GameOver()
    {
        SceneManager.sceneLoaded += OnEndScreenLoaded;
        SceneManager.LoadScene("LoseScreen");
    }

    private void WinGame()
    {
        SceneManager.sceneLoaded += OnEndScreenLoaded;
        SceneManager.LoadScene("WinScreen");
    }

    // Runs after Win/Lose scene is loaded
    private void OnEndScreenLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "WinScreen" && scene.name != "LoseScreen") return;

        TMP_Text endScoreText = null;
        TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text tmp in allTMPTexts)
        {
            if (tmp.name == "ScoreText")
            {
                endScoreText = tmp;
                break;
            }
        }

        if (endScoreText != null)
        {
            endScoreText.text = "Score: " + points;
        }
        else
        {
            Debug.LogWarning("No TMP_Text named 'ScoreText' found on " + scene.name + "!");
        }

        // Unsubscribe after updating once
        SceneManager.sceneLoaded -= OnEndScreenLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
