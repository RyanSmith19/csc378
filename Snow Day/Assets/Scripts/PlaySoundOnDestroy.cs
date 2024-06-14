using UnityEngine;

public class PlaySoundOnDestroy : MonoBehaviour
{
    [SerializeField] private AudioSource destroySound;

    private void OnDestroy()
    {
        // Ensure the sound is played even if the object is destroyed
        if (destroySound != null)
        {
            // Create a temporary game object to play the sound
            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = destroySound.clip;
            audioSource.volume = destroySound.volume;
            audioSource.pitch = destroySound.pitch;
            audioSource.Play();

            // Destroy the temporary game object after the sound has finished playing
            Destroy(tempAudio, destroySound.clip.length);
        }
    }
}