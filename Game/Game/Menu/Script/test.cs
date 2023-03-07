using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public void LoadGame()
    {

        SceneManager.LoadScene(4);
    }

    public void Quitgame()
    {

        Application.Quit();
        Debug.Log("Test");
    }
}
