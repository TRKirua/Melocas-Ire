using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class ScriptMenuVid : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool SynopsisPlaying = false; 

    public GameObject AffichageMenu;

    public VideoPlayer Video;
    
    public GameObject PanelSyno; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SynopsisPlaying)
            {
                Reprise(); 
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && SynopsisPlaying)
        {
            if (Video.isPlaying)
                Video.Pause();
            else
            {
                Video.Play();
            }
        }

        if (Video.time >= Video.clip.length)
        {
            Reprise();
        }

        if (!SynopsisPlaying)
            PanelSyno.gameObject.SetActive(false); 
    }

    public void Reprise()
    {
        PanelSyno.gameObject.SetActive(false); 
        SynopsisPlaying = false; 
        Video.Pause();
        AffichageMenu.SetActive(true);
    }
    
    
    public void Synopsis()
    {
        PanelSyno.gameObject.SetActive(true); 
        SynopsisPlaying = true;
        Debug.Log("Loading Synopsis");
        Video.Play();
    }
    
}