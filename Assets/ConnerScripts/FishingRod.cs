using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform hookSpawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        }
    }
}
