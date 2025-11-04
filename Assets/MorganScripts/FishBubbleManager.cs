using UnityEngine;

public class FishBubbleManager : MonoBehaviour
{
    [Range(0f, 1f)] public float volume = 0.3f;
    public float minInterval = 0.5f;
    public float maxInterval = 1.5f;
    public int maxFishSounds = 5;

    private float nextPlayTime;

    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
            var fish = GameObject.FindGameObjectsWithTag("Fish");
            if (fish.Length > 0)
            {
                int count = Mathf.Min(maxFishSounds, fish.Length);
                for (int i = 0; i < count; i++)
                {
                    if (Random.value < 0.5f)
                        SoundManager.PlaySound(SoundType.FishSound, volume);
                }
            }

            nextPlayTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }
}