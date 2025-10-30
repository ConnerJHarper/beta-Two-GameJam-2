using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    public float reelDelay = 3f;       // when to start reeling up
    public float reelSpeed = 3f;       // speed of reeling
    public float lifetime = 5f;        // total time before despawning
    private bool isReelingUp = false;
    private Vector3 startPos;
    private LineRenderer lineRenderer;
    private Transform boat;

    private void Start()
    {
        startPos = transform.position;
        lineRenderer = GetComponent<LineRenderer>();

        // Find boat by tag
        GameObject boatObj = GameObject.FindGameObjectWithTag("Boat");
        if (boatObj != null)
            boat = boatObj.transform;

        // Line setup
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
        }

        // Start timers
        Invoke(nameof(StartReeling), reelDelay);
        Invoke(nameof(Despawn), lifetime); // 👈 Auto-despawn after set time
    }

    private void Update()
    {
        // Update line positions
        if (lineRenderer != null && boat != null)
        {
            lineRenderer.SetPosition(0, boat.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        // Reel upward
        if (isReelingUp)
        {
            transform.Translate(Vector2.up * reelSpeed * Time.deltaTime);
            if (transform.position.y >= startPos.y)
                Despawn();
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
            Destroy(collision.gameObject);
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(1);
        }

        Destroy(gameObject);
    }

    private void Despawn()
    {
        // Optional: fade out line instead of instantly disappearing
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        Destroy(gameObject);
    }
}
