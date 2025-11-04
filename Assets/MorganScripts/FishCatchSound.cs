using UnityEngine;

public class FishCatchSound : MonoBehaviour
{
    [Header("Catch Sound Settings")]
    public AudioClip catchClip;            // assign your coin / catch sound here
    [Range(0f, 1f)] public float volume = 1f;
    public float pitchIncrease = 0.2f;     // how much pitch rises per catch
    public float maxPitch = 2f;            // highest pitch allowed
    private static float comboPitch = 1f;
    public static void ResetCombo()
    {
        comboPitch = 1f;
        Debug.Log("FishCatchSound combo reset.");
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