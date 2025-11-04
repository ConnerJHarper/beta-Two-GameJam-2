using UnityEngine;

public class FishCatchSound : MonoBehaviour
{
    [Header("Catch Sound Settings")]
    public AudioClip catchClip;
    [Range(0f, 1f)] public float volume = 1f; // Volume Slider
    public float pitchIncrease = 0.25f; // Pitch / Combo Increase Increments
    public float maxPitch = 2f; // Max Pitch / Combo Sound
    private static float comboPitch = 1f;
    public static void ResetCombo()
    {
        comboPitch = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fish")) return;
        if (catchClip == null) return;
        float pitchToPlay = comboPitch;
        comboPitch = Mathf.Min(comboPitch + pitchIncrease, maxPitch);
        GameObject temp = new GameObject("FishCatchSoundTemp");
        AudioSource s = temp.AddComponent<AudioSource>();
        s.clip = catchClip;
        s.volume = volume;
        s.pitch = pitchToPlay;
        s.spatialBlend = 0f;
        s.Play();
        Destroy(temp, catchClip.length);
    }
}