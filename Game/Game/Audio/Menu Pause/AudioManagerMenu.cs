using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMenu : MonoBehaviour
{
    
    public AudioClip[] playlist;
    public AudioSource audioSource;
    private int musicIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
            PlayNextSong();
        if (ScriptMenuVid.SynopsisPlaying)
            audioSource.Pause();
        if (!ScriptMenuVid.SynopsisPlaying)
            audioSource.UnPause();
    }
    
    void PlayNextSong()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }
}
