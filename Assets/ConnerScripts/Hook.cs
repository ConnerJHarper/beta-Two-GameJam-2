using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Hook : MonoBehaviour
{
    public float reelDelay = 3f;       
    public float reelSpeed = 3f;       
    public float lifetime = 5f;        
    private bool isReelingUp = false;
    private Vector3 startPos;
    private LineRenderer lineRenderer;
    private Transform boat;
    private bool caughtFish = false;

    private void Start()
    {
        startPos = transform.position;
        lineRenderer = GetComponent<LineRenderer>();

        
        GameObject boatObj = GameObject.FindGameObjectWithTag("Boat");
        if (boatObj != null)
            boat = boatObj.transform;

        
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
        }

        
        Invoke(nameof(StartReeling), reelDelay);
        Invoke(nameof(Despawn), lifetime); 
    }

    private void Update()
    {
        
        if (lineRenderer != null && boat != null)
        {
            lineRenderer.SetPosition(0, boat.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        
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
            Destroy(collision.gameObject);
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(1);
        }

        Destroy(gameObject);
    }

    private void Despawn()
    {
        if (!caughtFish)
        {
            FishCatchSound.ResetCombo();
        }

        
        if (lineRenderer != null)
            lineRenderer.enabled = false;

        Destroy(gameObject);
    }
}
