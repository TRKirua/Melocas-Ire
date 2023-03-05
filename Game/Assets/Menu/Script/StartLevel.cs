using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class StartLevel : MonoBehaviour
{
    public void Loadlevelone()
    {
        SceneManager.LoadScene(0);
    }

    public void Loadleveltwo()
    {
        SceneManager.LoadScene(1);  
    }
}
