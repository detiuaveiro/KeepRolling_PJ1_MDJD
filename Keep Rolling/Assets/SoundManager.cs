using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioClip clickButton;

    public AudioSource source;

    public void PlayClickButtonSound()
    {
        source.clip = clickButton;
        source.Play();
    }
}
