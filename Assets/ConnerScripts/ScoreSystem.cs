using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public int score;
    public Text scoreText;

    public void AddTenPoints()
    {
        score += 10;
        scoreText.text = "Score" + score;
    }
}
