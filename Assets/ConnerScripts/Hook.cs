using UnityEngine;

public class Hook : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            Debug.Log("Fish Caught!");
            Destroy(collision.gameObject);
        }
    }
}
