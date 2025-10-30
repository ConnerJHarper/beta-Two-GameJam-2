using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float range = 3f;
    private Vector3 startPos;
    private int direction = 1;
    public bool isCombo = false;
    public int combo = 0;
    public Text comboText;


    void Start()
    {
        startPos = transform.position;
        
    }

    void Update()
    {
        
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Check if the fish has reached the movement range
        if (Vector2.Distance(startPos, transform.position) > range)
        {
            direction *= -1; 
            Flip();             
        }

        if (isCombo)
        {
            speed += 1f; 


        }

    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Combo()
    {

    }
}
