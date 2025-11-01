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

    [Header("Combo UI (optional)")]
    public bool isCombo = false;
    public int combo = 0;
    public Text comboText;

    void Start()
    {
        startPos = transform.position;
        baseSpeed = speed; // Store original speed
    }

    void Update()
    {
        // Move horizontally
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Flip when out of range
        if (Vector2.Distance(startPos, transform.position) > range)
        {
            direction *= -1;
            Flip();
        }

        // Optional: show combo text
        if (isCombo && comboText != null)
        {
            comboText.text = "Combo: " + combo;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Called by GameManager when combo rank increases
    public void BoostSpeed(float multiplier)
    {
        speed = baseSpeed * multiplier;
    }

    // Called by GameManager when combo resets
    public void ResetSpeed()
    {
        speed = baseSpeed;
    }
}