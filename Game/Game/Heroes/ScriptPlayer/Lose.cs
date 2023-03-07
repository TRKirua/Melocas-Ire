using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : MonoBehaviour
{
    private int counter;
    private GameObject[] players;
    public int NumeroDuLevel;
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<DeathPacito>().isDead)
            {
                player.GetComponent<DeathPacito>().isDead = false;
                counter += 1;
            }
        }
        
        if (counter == 3)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0.000001f;
            StartCoroutine(WaitReloadScene());
            counter = 0;
        }
    }
    
    IEnumerator WaitReloadScene()
    {
        yield return new WaitForSeconds(0.000005f);
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Level " + NumeroDuLevel);
    }
}
