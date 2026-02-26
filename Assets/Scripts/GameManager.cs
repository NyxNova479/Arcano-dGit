using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

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

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this; ;
        }
        else
        {
            Destroy(gameObject);
        }

        controls = new InputSystem_Actions();
        //controls.UI.Pause.performed += ctx => Pause();

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
        //controls.UI.Pause.Enable();
    }

    private void OnDisable()
    {
        //controls.UI.Pause.Disable();
    }

    private void Pause()
    {
        IsPaused = !IsPaused;
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
            ballBehaviour.isLaunched = false;
            _livesText.text = _lives.ToString();

        }
        //if (_livesImage == null) return;
        //else if (_lives == 2)
        //{
        //    _livesImage[1].SetActive(false);
        //}
        //else _livesImage[0].SetActive(false);
    }

    private IEnumerator ShowGameOver()
    {
        
        gameOverPanel.SetActive(true);
        //audioSource.PlayOneShot(gameOverSound);
        for (int i = 0; i < "Game Over".Length; i++)
        {
            gameOverUI.text += "Game Over"[i];
            yield return new WaitForSeconds(0.5f);
        }
        Time.timeScale = 0f;
    }

    public void CompletedLevel()
    {
        // TODO : Implémenter le CompletedLevel
        bricksManager.SpawnBricks();
        
        _lives = 3;
    }

    private void SaveScore()
    {
        if (score < highScore) return;

        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();

    }


}


