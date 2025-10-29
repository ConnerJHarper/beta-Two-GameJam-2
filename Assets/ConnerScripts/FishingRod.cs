using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform hookSpawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Drop a new hook every time space is pressed
            Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        }
    }
}
