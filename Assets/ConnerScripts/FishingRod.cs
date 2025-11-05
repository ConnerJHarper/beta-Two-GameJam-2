using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform hookSpawnPoint;

    void Update()
    {
        // Only cast a new hook if none is active
        if (Input.GetKeyDown(KeyCode.Space) && !Hook.IsHookActive)
        {
            Instantiate(hookPrefab, hookSpawnPoint.position, Quaternion.identity);
        }
    }
}
