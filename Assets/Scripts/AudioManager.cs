using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip shootSound;

    void Start()
    {
        PlayMusic();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlayShootSound();
        }
    }

    // Call this method to start playing the background music on a loop
    void PlayMusic()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Call this method to play the shooting sound effect once
    void PlayShootSound()
    {
        sfxSource.PlayOneShot(shootSound);
    }
}