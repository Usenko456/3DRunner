using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip Background;
    public AudioClip Coincollected;
    public AudioClip Finish;
    public AudioClip Lose;
    
    private void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }
    // Method for playing sound effects
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
