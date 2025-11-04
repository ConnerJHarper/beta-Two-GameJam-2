using UnityEngine;

public class PlayExampleSound : MonoBehaviour
{
    public void PlaySound()
    {
        SoundManager.PlaySound(SoundType.BoatMove);
    }
}