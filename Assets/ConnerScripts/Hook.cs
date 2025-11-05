using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    [Header("Hook Settings")]
    public float reelDelay = 0.3f;       // Delay before reeling starts
    public float reelSpeed = 5f;         // Speed at which hook reels toward boat
    public float lifetime = 3f;          // Max time hook exists if no fish is caught

    private bool isReeling = false;
    private bool caughtFish = false;

    private Vector3 startPos;
    private LineRenderer lineRenderer;
    private Transform boat;

    // Prevent multiple hooks
    public static bool IsHookActive = false;

    private void Start()
    {
        // Prevent multiple hooks
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

        // Auto-despawn hook if it doesn't catch anything
        Invoke(nameof(Despawn), lifetime);
    }

    private void Update()
    {
        // Draw the fishing line from boat to hook
        if (lineRenderer != null && boat != null)
        {
            lineRenderer.SetPosition(0, boat.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        // Smooth reeling motion toward boat
        if (isReeling && boat != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, boat.position, reelSpeed * Time.deltaTime);

            // If hook reaches the boat, despawn
            if (Vector3.Distance(transform.position, boat.position) < 0.01f)
                Despawn();
        }
    }

    private void StartReeling()
    {
        isReeling = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            caughtFish = true;

            // Add score based on fish prefab
            FishMovement fish = collision.GetComponent<FishMovement>();
            if (fish != null && GameManager.Instance != null)
                GameManager.Instance.AddScore(fish.scoreValue);

            // Destroy the fish
            Destroy(collision.gameObject);

            // Allow a new hook to be cast immediately
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
