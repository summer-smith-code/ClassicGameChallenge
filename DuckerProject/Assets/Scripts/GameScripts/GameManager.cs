using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Needed for TextMeshPro

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
    public TMP_Text scoreText; // Auto-detected if not assigned

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

    private void Start()
    {
        if (currentLives == 0)
            ResetGame();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ConnectLifeUI();
        ConnectScoreUI();   // Auto-detect TMP text by name
        UpdateLivesUI();
        UpdateScoreUI();    // Refresh the score display
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
            // Find all TMP_Text objects in the scene
            TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>();

            // Pick the one named "ScoreText"
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

    public void CoveFilled()
    {
        covesFilled++;
        points += 50; // 50 points per cove
        UpdateScoreUI(); // Update score after collecting a cove

        Debug.Log("Cove filled! Total coves: " + covesFilled);

        if (covesFilled >= totalCoves)
        {
            points += 1000; // Bonus for filling all coves
            UpdateScoreUI(); // Update score for bonus
            WinGame();
        }
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

    private void GameOver() => SceneManager.LoadScene("LoseScreen");

    private void WinGame()
    {
        points += currentLives * 200; // Bonus for unused lives
        UpdateScoreUI(); // Update score before going to win screen
        SceneManager.LoadScene("WinScreen");
    }
}
