using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const int LIVES = 3;

    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] TextMeshProUGUI txtMaxScore;
    [SerializeField] TextMeshProUGUI txtMessage;
    [SerializeField] GameObject[] imgLives;
    [SerializeField] int extraLifePoints = 1000;

    int score;
    int maxScore;
    int lives = LIVES;
    int nextExtraLifeScore;

    static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        nextExtraLifeScore = extraLifePoints;
        txtMessage.gameObject.SetActive(false);
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].SetActive(i < lives);
        }

        txtScore.text = string.Format("{0,4:D4}", score);
        txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
    }

    public void AddScore(int points)
    {
        score += points;

        if (score > maxScore)
        {
            maxScore = score;
        }

        if (score >= nextExtraLifeScore)
        {
            AddLife();
            nextExtraLifeScore += extraLifePoints;
        }

        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            lives = 0;
            UpdateUI();
            GameOver();
            return;
        }

        UpdateUI();
    }

    public void AddLife()
    {
        if (lives < LIVES)
        {
            lives++;
        }

        UpdateUI();
    }

    public bool HasLives()
    {
        return lives > 0;
    }

    void GameOver()
    {
        txtMessage.gameObject.SetActive(true);
        txtMessage.text = "GAME OVER";
    }

    public void ResetGame()
    {
        score = 0;
        maxScore = 0;
        lives = LIVES;
        nextExtraLifeScore = extraLifePoints;
        txtMessage.gameObject.SetActive(false);
        UpdateUI();
    }
}