using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    public AudioSource AudioSource;
    private float musicvolume = 1f;

    void start()
    {
        
        AudioSource.Play();
    }

    void Update()
    {
        AudioSource.volume = musicvolume;

    }

    public void updateVolume(float volume)
    {
        musicvolume = volume;

    }

}
