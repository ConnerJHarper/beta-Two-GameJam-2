using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public GameObject hookPrefab;
    public Transform hookSpawn;
    private GameObject currentHook;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (currentHook == null)
            {
                // Drop Hook
                currentHook = Instantiate(hookPrefab, hookSpawn.position, Quaternion.identity);
            }
            else
            {
                // Reel In (destroy hook)
                Destroy(hookPrefab);
            }
        }
    }
}
