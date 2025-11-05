using UnityEngine;
using UnityEngine.UI;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float range = 3f;

    private Vector3 startPos;
    private int direction = 1; // 1 = right, -1 = left
    private float baseSpeed;

    [Header("Score Settings")]
    [Tooltip("Points this fish gives when caught")]
    public int scoreValue = 10;

    [Header("Combo UI (optional)")]
    public bool isCombo = false;
    public int combo = 0;
    public Text comboText;

    void Start()
    {
        startPos = transform.position;
        baseSpeed = speed;

        // Random initial direction
        direction = Random.value > 0.5f ? 1 : -1;

        // Ensure sprite faces correct initial direction
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void Update()
    {
        MoveFish();
        UpdateComboText();
    }

    private void MoveFish()
    {
        // Move fish horizontally
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Reverse direction if exceeded range
        if (Vector2.Distance(startPos, transform.position) > range)
        {
            direction *= -1;
            startPos = transform.position; // reset start position for next range
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

    // Click-to-catch handler
    void OnMouseDown()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(scoreValue);

        Destroy(gameObject);
    }
}
