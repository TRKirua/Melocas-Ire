using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiallogueTrigger : MonoBehaviour
{
    public string name;
    
    [TextArea(3,10)]
    public string[] sentences;

    public Color oldcolor;
    public List<GameObject> listOfPlayers;
    public float PositionDeuxièmePortailBoss;
    private bool hasteleported;
    private bool firsttime = true;
    private float WaitTime = 3;

    public DiallogueTrigger (string name, string[] sentences)
    {
        this.name = name;
        this.sentences = sentences;
    }

    void FixedUpdate()
    {
        if (!GetComponent<DiallogueManager>().HasEnded)
        {
            DiallogueTrigger diallo = this;

            foreach (GameObject player in listOfPlayers)
            {
                if (Mathf.Abs(player.transform.position.x) >= Mathf.Abs(PositionDeuxièmePortailBoss))
                    hasteleported = true;
            }

            if (hasteleported)
            {
                GetComponent<DiallogueManager>().nameText.color = Color.red;
                GetComponent<DiallogueManager>().diallogueText.color = Color.white;
                GetComponent<DiallogueManager>().BoxText.color = oldcolor;

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

    public void TriggerDiallogue(DiallogueTrigger dialogue)
    {
        FindObjectOfType<DiallogueManager>().StartDiallogue(dialogue);
    }
}
