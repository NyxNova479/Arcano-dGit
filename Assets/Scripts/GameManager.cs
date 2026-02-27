using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager Instance;

    public bool IsPaused = false;

    private InputSystem_Actions controls;

    [SerializeField]
    private int score = 0;

    public int highScore = 0;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private BallBehaviour ballBehaviour;

    [SerializeField]
    private TextMeshProUGUI _scoreUI;
    [SerializeField]
    private TextMeshProUGUI _highScore;
    [SerializeField]
    private TextMeshProUGUI _livesText;
    [SerializeField]
    private GameObject[] _livesImage;

    public BricksManager bricksManager;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private TextMeshProUGUI gameOverUI;
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField] AudioClip levelUpSound;
    [SerializeField] AudioClip gameOverSound;
    private AudioSource audioSource;

    private bool isGameOver = false;
    private bool restarted = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this; ;
        }
        else
        {
            Destroy(gameObject);
        }

        controls = new InputSystem_Actions();
        controls.UI.Pause.performed += ctx => Pause();

        _livesText.text = _lives.ToString();
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("Score", score);

        if (highScore.ToString().Length <= 0)
        {
            _highScore.text = "0000";
            return;
        }

        if (highScore.ToString().Length <= 2)
        {
            _highScore.text = "00" + highScore.ToString();
            return;
        }

        if (highScore.ToString().Length == 3)
        {
            _highScore.text = "0" + highScore.ToString();
            return;
        }


        _highScore.text = highScore.ToString();
    }

    private void OnEnable()
    {
        controls.UI.Pause.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Pause.Disable();
    }

    private void Update()
    {
        if (isGameOver && Input.anyKeyDown)
        {
            RestartGame();
            restarted = true;
        }
    }

    private void Pause()
    {
        IsPaused = !IsPaused;

        pausePanel.SetActive(IsPaused);
        Time.timeScale = IsPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        pausePanel.SetActive(IsPaused);
        Time.timeScale = 1f;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void AddScore(int points)
    {

        score += points;



        if (score.ToString().Length <= 2)
        {
            _scoreUI.text = "00" + score.ToString();
            return;
        }

        if (score.ToString().Length == 3)
        {
            _scoreUI.text = "0" + score.ToString();
            return;
        }


        _scoreUI.text = score.ToString();

    }




    private void ResetScore() => score = 0;


    public void LoseLife()
    {
        _lives--;


        if (_lives <= 0)
        {
            _livesText.text = "0";
            ballBehaviour.gameObject.SetActive(false);
            StartCoroutine(ShowGameOver());
        }
        else
        {


            playerPrefab.transform.position = new Vector2(0f, -4f);
            ballBehaviour.gameObject.SetActive(false);
            ballBehaviour.isLaunched = false;
            ballBehaviour.gameObject.SetActive(true);
            _livesText.text = _lives.ToString();

        }

    }

    private IEnumerator ShowGameOver()
    {
        audioSource.PlayOneShot(gameOverSound, 0.5f);
        isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverUI.text = "";

        for (int i = 0; i < "Game Over".Length; i++)
        {
            gameOverUI.text += "Game Over"[i];
            yield return new WaitForSeconds(0.3f);
        }

        gameOverUI.text += "\n\n Press Any Key To Restart";

        SaveScore();

        if(!restarted) Time.timeScale = 0f;
    }

    public void CompletedLevel()
    {

        ballBehaviour.isLaunched = false;
        ballBehaviour.ballSpeed += Random.Range(-2, 0.5f);
        audioSource.PlayOneShot(levelUpSound, 0.6f);
        bricksManager.SpawnBricks();
        
        _lives = 3;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        gameOverPanel.SetActive(false);
        gameOverUI.text = "";

        // Reset score
        score = 0;
        _scoreUI.text = "0000";

        // Reset vies
        _lives = 3;
        _livesText.text = _lives.ToString();

        // Reset player
        playerPrefab.transform.position = new Vector2(0f, -4f);

        // Reset balle
        ballBehaviour.gameObject.SetActive(false);
        ballBehaviour.isLaunched = false;
        ballBehaviour.gameObject.SetActive(true);

        // Reset bricks
        bricksManager.ClearBricks();   // 👈 on va l'ajouter
        bricksManager.SpawnBricks();
    }

    private void SaveScore()
    {
        if (score < highScore) return;

        highScore = score;
        _highScore.text = highScore.ToString("0000");

        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();

    }


}


