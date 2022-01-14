using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
    
    public AudioSource audioSource;
    public AudioClip audioClip;

    public void PlayAudio()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
