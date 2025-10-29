using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float range = 3f;
    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Move horizontally
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Check if the fish has reached the movement range
        if (Vector2.Distance(startPos, transform.position) > range)
        {
            direction *= -1; // Reverse direction
            Flip();          // Flip the sprite visually
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
