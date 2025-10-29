using JetBrains.Annotations;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public float reelDelay = 3f;
    public float reelSpeed = 3f;
    private bool isReelingUp = false;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
        Invoke(nameof(StartReeling), reelDelay);
    }

    private void Update()
    {
        if (isReelingUp)
        {
            transform.Translate(Vector2.up * reelSpeed * Time.deltaTime);

            if (transform.position.y >= startPos.y)
            {
                Destroy(gameObject);
            }
        }
    }

    private void StartReeling()
    {
        isReelingUp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish caught!");
            Destroy(collision.gameObject);

            Destroy(gameObject);
            

            // ?? Add to score
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(1);
        }
    }
}
