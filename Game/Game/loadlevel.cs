using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadlevel : MonoBehaviour
{
    public void Loadlevel1()
    {

        SceneManager.LoadScene(2);
    }

    public void LoadLevel2()
    {

        SceneManager.LoadScene(3);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene(9);
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene(10);
    }

    public void Level5()
    {

        SceneManager.LoadScene(11);
    }
}
