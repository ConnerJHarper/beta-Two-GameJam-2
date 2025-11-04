using UnityEngine;

public class BoatMovementSound : MonoBehaviour
{
    [Header("Boat Movement Sound Settings")]
    [Range(0f, 1f)] public float volume = 0.8f; // Volume Slider
    public float playDelay = 0.4f; // Delay Time

    private bool wasMoving = false;
    private float lastPlayTime = -999f; // First movement plays sound instantly

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(move) > 0.01f;

        if (isMoving && !wasMoving)
        {
            // Delays only after you move the first time
            if (Time.time - lastPlayTime > playDelay || lastPlayTime < 0f)
            {
                SoundManager.PlaySound(SoundType.BoatMove, volume);
                lastPlayTime = Time.time;
            }
        }

        wasMoving = isMoving;
    }
}