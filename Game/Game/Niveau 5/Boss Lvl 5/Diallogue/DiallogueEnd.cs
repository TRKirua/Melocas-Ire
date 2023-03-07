using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class DiallogueEnd : MonoBehaviour
{
    public string name;
    
    [TextArea(3,10)]
    public string[] sentences;

    public Color oldcolor;
    private bool firsttime = true;
    private float WaitTime = 3;
    private GameObject boss;

    private void Start()
    {
        boss = GameObject.FindWithTag("boss");
    }

    
    public DiallogueEnd (string name, string[] sentences)
    {
        this.name = name;
        this.sentences = sentences;
    }

    void FixedUpdate()
    {
        if (!GetComponent<DiallogueEndManager>().HasEnded)
        {
            DiallogueEnd diallo = this;

            if (boss.GetComponent<EnnemyS>().health < 1)
            {
                GetComponent<DiallogueEndManager>().nameText.color = Color.red;
                GetComponent<DiallogueEndManager>().diallogueText.color = Color.white;
                GetComponent<DiallogueEndManager>().BoxText.color = oldcolor;

                if (firsttime)
                {
                    TriggerDiallogue(diallo);
                    firsttime = false;
                }

                if (WaitTime < 0)
                {
                    WaitTime = 3;
                    TriggerDiallogue(diallo);
                }
                else
                    WaitTime -= 0.02f;
            }
        }
    }

    public void TriggerDiallogue(DiallogueEnd dialogue)
    {
        FindObjectOfType<DiallogueEndManager>().StartDiallogue(dialogue);
    }
}