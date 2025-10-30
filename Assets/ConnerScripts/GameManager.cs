using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public Text scoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        amount = 10;

        score += amount;
        Debug.Log("Score" + score);

        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        scoreText.text = "Score: " + score;
    }
}
