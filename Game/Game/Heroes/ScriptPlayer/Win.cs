using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class Win : MonoBehaviour
{
    private GameObject boss;
    public GameObject pauseMenuUI;
    public int NumeroDuLevelSuivant;

    // Update is called once per frame
    private void Start()
    {
       boss = GameObject.FindWithTag("boss");
    }

    void Update()
    {
        if (boss.GetComponent<EnnemyS>().health < 1)
            StartCoroutine(WaitMenu());
    }
    
    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Scene Menu"); 
    }

    public void Continue()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level " + NumeroDuLevelSuivant);
    }

    IEnumerator WaitMenu()
    {
        yield return new WaitForSeconds(2);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}