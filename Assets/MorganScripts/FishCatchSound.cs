using UnityEngine;

public class HookCatchSound : MonoBehaviour
{
    [Header("Fish Catch Sound Settings")]
    [Range(0f, 1f)] public float volume = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            SoundManager.PlaySound(SoundType.FishCatch, volume);
        }
    }
}