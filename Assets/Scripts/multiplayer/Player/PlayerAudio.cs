using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip[] audioClips;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayPickupSound(float volume = 1.0f)
    {
        PlaySound(audioClips[0], volume);
    }

    public void PlayDropitemSound(float volume = 0.5f)
    {
        PlaySound(audioClips[1], volume);
    }

    public void SpraySound(float volume = 1.0f)
    {
        PlaySound(audioClips[2], volume);
    }

    public void JetpackSound(float volume = 0.4f)
    {
        PlaySound(audioClips[3], volume);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    public void CreatePortalSound(float volume = 1.0f)
    {
        PlaySound(audioClips[4], volume);
    }
}
