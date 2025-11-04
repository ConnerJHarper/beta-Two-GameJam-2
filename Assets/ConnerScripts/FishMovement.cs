using UnityEngine;
using UnityEngine.UI;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float range = 3f;

    private Vector3 startPos;
    private int direction = 1;
    private float baseSpeed;

    [Header("Score Settings")]
    [Tooltip("How many points this fish gives when caught")]
    public int scoreValue = 10;

    [Header("Combo UI (optional)")]
    public bool isCombo = false;
    public int combo = 0;
    public Text comboText;

    void Start()
    {
        startPos = transform.position;
        baseSpeed = speed;
        direction = Random.value > 0.5f ? 1 : -1; 
    }

    void Update()
    {
        MoveFish();
        UpdateComboText();
    }

    private void MoveFish()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) > range)
        {
            transform.position = startPos + Vector3.right * range * direction;
            direction *= -1;
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    private void UpdateComboText()
    {
        if (isCombo && comboText != null)
            comboText.text = "Combo: " + combo;
    }

    public void BoostSpeed(float multiplier)
    {
        speed = baseSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        speed = baseSpeed;
    }

   
}
