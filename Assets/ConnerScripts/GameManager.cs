using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public Text scoreText;

    public bool combo;

    public GameObject[] rankImages;
    public Text comboText; 

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

    public void Update()
    {
        ChangeComboUI();

        ChangeRanks();
    }

    public void AddScore(int amount)
    {
        amount = 10;

        score += amount;
        Debug.Log("Score" + score); 

        UpdateScoreUI();

        
    }

    public void ChangeComboUI()
    {
        if (score == 30)
        comboText.text = "Combo: " + comboText;

    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        scoreText.text = "Score: " + score; 
    }

    public void ChangeRanks()
    {
        if (score >= 30)
            rankImages[1] = new GameObject();



    }
}
