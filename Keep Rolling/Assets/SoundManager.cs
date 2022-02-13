using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip clickButton;
    public AudioClip placeObjectButton;
    public AudioClip winSound;
    public AudioClip loseSound;

    public AudioSource source;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    public void PlayClickButtonSound()
    {
        source.clip = clickButton;
        source.Play();
    }

    public void PlayPlaceObjectSound()
    {
        source.clip = placeObjectButton;
        source.Play();
    }

    public void PlayWinSound()
    {
        source.clip = winSound;
        source.Play();
    }
    public void PlayLoseSound()
    {
        source.clip = loseSound;
        source.Play();
    }
}
