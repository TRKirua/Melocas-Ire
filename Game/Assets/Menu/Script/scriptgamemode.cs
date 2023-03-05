using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scriptgamemode : MonoBehaviour
{
    public void LoadSolo()
    {
        SceneManager.LoadScene(2);
    }

    public void Back1()
    {
        SceneManager.LoadScene(0);

    }
}
