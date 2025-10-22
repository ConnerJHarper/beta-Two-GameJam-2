using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSystem : MonoBehaviour
{
    public int score;
    public Text scoreText;
    public int combo;
    public Animation comboAnim;

    public void Start()
    {
        scoreText = GetComponent<Text>();

        comboAnim = GetComponent<Animation>();  

        foreach (AnimationState state in comboAnim)
        {
            state.speed = 0.5f;
        }
    }

    public void AddTenPoints()
    {
        score += 10;
        scoreText.text = "Score" + score;
    }

    private void Update()
    {
        if (score == 50)
        {
            ComboBoost();
        }

    }
    public void ComboBoost()
    {
        score += combo;  
        comboAnim.Play();
    }
}
