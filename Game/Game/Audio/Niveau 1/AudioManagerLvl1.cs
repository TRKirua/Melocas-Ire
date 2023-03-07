using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerLvl1 : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audioSource;
    public Transform Player; 
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
        if (!audioSource.isPlaying && Player.position.x < 400f)
            PlayNextSong();
        else if (Player.position.x > 400f && musicIndex != playlist.Length-1) // position à l'arrivée de la salle du boss
        {
            PlayMusicBoss();
        }
        if (ScriptMenuPause.SynopsisPlaying)
            audioSource.Pause();
        if (!ScriptMenuPause.SynopsisPlaying)
            audioSource.UnPause();
    }

    void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % (playlist.Length-1);
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }

    void PlayMusicBoss()
    {
        musicIndex = playlist.Length-1;
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }
}