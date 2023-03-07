using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ScriptMenuPause : MonoBehaviour
{
    public static bool GamePaused = false;
    public static bool SynopsisPlaying = false; 

    public GameObject MenuPause;

    public VideoPlayer Video;
    
    public GameObject PanelSyno; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Reprise(); 
            }
            else
            {
                Pause();
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
        MenuPause.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    void Pause()
    {
        MenuPause.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
    
    public void Synopsis()
    {
        PanelSyno.gameObject.SetActive(true); 
        SynopsisPlaying = true;
        Debug.Log("Loading Synopsis");
        Video.Play();
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene Menu"); 
    }

    public void ExitGame()
    {
        Debug.Log("Bye Bye game !! ");
        Application.Quit();
    }
}
