using UnityEngine;

public class DeathSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDeathSound()
    {
        audioSource.Play();
    }
}
