using UnityEngine;

public class HookDropSound : MonoBehaviour
{
    [Header("Hook Drop Sound Settings")]
    [Range(0f, 1f)] public float volume = 0.8f; // Volume Slider
    public float playDelay = 0.5f; // Delay

    private static float lastPlayTime = -999f;

    void Start()
    {
        if (Time.time - lastPlayTime > playDelay)
        {
            SoundManager.PlaySound(SoundType.HookDrop, volume);
            lastPlayTime = Time.time;
        }
    }
}