using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassBackground : MonoBehaviour
{
    public Renderer rend;
    

    void Update()
    {
        if (transform.position.x > 55 && transform.position.x < 141)
            rend.sortingOrder = 3;
        else 
        {
            if (transform.position.x > 222 && transform.position.x < 233)
                rend.sortingOrder = 3;
        
            else 
            {
                if (transform.position.x > 335 && transform.position.x < 400)
                    rend.sortingOrder = 3;
                else
                    rend.sortingOrder = 10;
            }
        }
    }
}
