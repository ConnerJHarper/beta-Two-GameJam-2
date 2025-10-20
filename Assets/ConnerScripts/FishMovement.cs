using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float range = 3f;
    private Vector2 startPos;
    private int direction = 1;


    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        if (Vector2.Distance(startPos, transform.position) > range)
        {
            direction *= -1;
        }
    }
}
