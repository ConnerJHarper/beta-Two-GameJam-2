using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    [Header("Hook Settings")]
    public float reelDelay = 0.5f;       
    public float reelSpeed = 5f;         
    public float lifetime = 3f;         

    private bool isReelingUp = false;
    private Vector3 startPos;
    private LineRenderer lineRenderer;
    private Transform boat;
    private bool caughtFish = false;

    // Prevent multiple hooks
    public static bool IsHookActive = false;

    private void Start()
    {
        // If another hook is active, destroy this one immediately
        if (IsHookActive)
        {
            Destroy(gameObject);
            return;
        }

        IsHookActive = true;

        startPos = transform.position;
        lineRenderer = GetComponent<LineRenderer>();

        // Find boat
        GameObject boatObj = GameObject.FindGameObjectWithTag("Boat");
        if (boatObj != null)
            boat = boatObj.transform;

        // Configure line renderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
        }

        // Start reeling after a short delay
        Invoke(nameof(StartReeling), reelDelay);

        // Despawn hook automatically if it doesn't catch a fish
        Invoke(nameof(Despawn), lifetime);
    }

    private void Update()
    {
        // Draw line from boat to hook
        if (lineRenderer != null && boat != null)
        {
            lineRenderer.SetPosition(0, boat.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        // Reel hook up
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
            caughtFish = true;

            // Give score from fish prefab
            FishMovement fish = collision.GetComponent<FishMovement>();
            if (fish != null && GameManager.Instance != null)
                GameManager.Instance.AddScore(fish.scoreValue);

            Destroy(collision.gameObject);

            // Hook can be cast again immediately
            IsHookActive = false;
            Destroy(gameObject);
        }
    }

    private void Despawn()
    {
        // Reset hook availability
        IsHookActive = false;

        // Disable line
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        Destroy(gameObject);
    }
}
