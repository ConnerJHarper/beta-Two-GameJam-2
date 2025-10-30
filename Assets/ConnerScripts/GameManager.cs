using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public Text scoreText;

    public Text comboText;
    public bool combo;


    public Image rankImageA;
    public Image rankImageB;
    public Image rankImageC;
    public Image rankImageD;
    public Image rankImageE;

    bool isASet;
    bool isBSet;

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
        if (score == 30)
        {
            isASet = true;
        }

        if (isASet = true && score == 90)
        {
            isBSet = true;
            isASet = false;
        }
    }
}
