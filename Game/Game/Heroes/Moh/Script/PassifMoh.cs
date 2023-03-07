using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = System.Numerics.Vector3;

public class PassifMoh : MonoBehaviour
{
    private float recharge = 7; //temps de recharge
    public bool hasShield; //a le bouclier ou non
    private bool mustGenerate; //si on doit faire apparaître le bouclier
    public GameObject passifMoh; //animation

    private bool isHit; //est frappé
    private int health; //vie
    public Transform passifCenter;
    private GameObject tamere;

    private void Start()
    {
        health = GetComponent<Health>().currentHealth;
    }

    public bool GetHit()
    {
        isHit = false;
        
        if (GetComponent<Health>().currentHealth != health)
        {
            health = GetComponent<Health>().currentHealth;
            isHit = true;
        }

        return isHit;
    }
    
    
    private void FixedUpdate()
    {
        if (mustGenerate)
        {
            tamere = Instantiate(passifMoh, passifCenter);
            hasShield = true;
            mustGenerate = false;
        }

        else
        {
            if (hasShield)
            {
                if (GetHit())
                {
                    hasShield = false;
                    tamere.GetComponent<PassifMoh2>().destroy = true;
                }

                recharge = 7;
            }

            else
            {
                if (recharge < 0)
                    mustGenerate = true;
                else
                    recharge -= 0.02f;
            }
        }
    }
}
