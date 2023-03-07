using System;
using System.Collections;
using System.Collections.Generic;
using ScriptPlayer;
using UnityEngine;

public class BonusMoh : MonoBehaviour
{
    public Collider2D collider;
    private LayerMask heroes;
    private Vector2 pos;

    private float WaitTimeAttack = 10;
    private float WaitTimeSpeed = 20;
    private float WaitTimeHealth = 100;

    private bool HadAttack, HadSpeed, HadHealth;
    private Color newcolor, oldcolor;
    public Renderer renderer;

    private void Start()
    {
        heroes = LayerMask.GetMask("Player");
        oldcolor = renderer.material.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = new Vector2(collider.transform.position.x, collider.transform.position.y);
        Collider2D[] Player = Physics2D.OverlapCircleAll(pos, 1, heroes);

        if (HadAttack)
        {
            if (WaitTimeAttack < 0)
            {
                HadAttack = false;
                
                for (int i = 0; i < Player.Length; i++)
                {
                    Player[i].GetComponent<Attack1Moh>().BasicAttack -= 2;
                    
                    GameObject magikprojekt = Player[i].GetComponent<Attack2Moh>().magicProjectile;
                    int MagicAttack = magikprojekt.GetComponent<ProjectileMoh>().MagicAttack;
                    MagicAttack -= 2;
                    Player[i].GetComponent<Attack2Moh>().magicProjectile.GetComponent<ProjectileMoh>().MagicAttack = MagicAttack;
                }
                
                renderer.material.color = oldcolor;
            }
            else
            {
                if (HadSpeed)
                {
                    if (ColorUtility.TryParseHtmlString("#A500FF", out newcolor))
                        renderer.material.color = newcolor;
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString("#FF0000", out newcolor))
                        renderer.material.color = newcolor;
                }

                WaitTimeAttack -= 0.02f;
            }
        }

        if (HadSpeed)
        {
            if (WaitTimeSpeed < 0)
            {
                HadSpeed = false;
                
                for (int i = 0; i < Player.Length; i++)
                {
                    Player[i].GetComponent<Move>().Speed -= 60;
                    Player[i].GetComponent<Move>().JumpForce -= 100;
                }
                
                renderer.material.color = oldcolor;
            }
            else
            {
                if (HadAttack)
                {
                    if (ColorUtility.TryParseHtmlString("#A500FF", out newcolor))
                        renderer.material.color = newcolor;
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString("#00E9FF", out newcolor))
                        renderer.material.color = newcolor;
                }

                WaitTimeSpeed -= 0.02f;
            }
        }

        if (HadHealth)
        {
            if (WaitTimeHealth < 0)
            {
                HadHealth = false;
                
                WaitTimeHealth = 100;
                renderer.material.color = oldcolor;
            }
            else
            {
                if (WaitTimeHealth <= 100 && WaitTimeHealth >= 50)
                {
                    if (ColorUtility.TryParseHtmlString("#00FF08", out newcolor))
                        renderer.material.color = newcolor;
                }
                else
                { 
                    if (WaitTimeHealth < 50 && WaitTimeHealth >= 25)
                        renderer.material.color = oldcolor;
                    else
                    {
                        if (ColorUtility.TryParseHtmlString("#00FF08", out newcolor))
                            renderer.material.color = newcolor;
                    }
                }

                WaitTimeHealth -= 3;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Collider2D[] Player = Physics2D.OverlapCircleAll(pos, 1, heroes);
            
        if (collider.tag == "attack")
        {
            GameObject[] potions = GameObject.FindGameObjectsWithTag("attack");

            if (!HadAttack)
            {
                for (int i = 0; i < Player.Length; i++)
                {
                    Player[i].GetComponent<Attack1Moh>().BasicAttack += 2;
                    
                    GameObject magikprojekt = Player[i].GetComponent<Attack2Moh>().magicProjectile;
                    int MagicAttack = magikprojekt.GetComponent<ProjectileMoh>().MagicAttack;
                    MagicAttack += 2;
                    Player[i].GetComponent<Attack2Moh>().magicProjectile.GetComponent<ProjectileMoh>().MagicAttack = MagicAttack;
                    
                    foreach (GameObject potion in potions)
                    {
                        float distplayerpot = Mathf.Abs(potion.transform.position.x - Player[i].transform.position.x);
                        if (distplayerpot < 5)
                            Destroy(potion);   
                    }
                }

                HadAttack = true;
            } 
            else
            {
                for (int i = 0; i < Player.Length; i++)
                {
                    foreach (GameObject potion in potions)
                    {
                        float distplayerpot = Mathf.Abs(potion.transform.position.x - Player[i].transform.position.x);
                        if (distplayerpot < 5)
                            Destroy(potion);
                    }
                }
            }
            
            WaitTimeAttack = 10;
        }

        if (collider.tag == "health")
        {
            GameObject[] potions = GameObject.FindGameObjectsWithTag("health");

            for (int i = 0; i < Player.Length; i++)
            {
                if (Player[i].GetComponent<Health>().currentHealth < Player[i].GetComponent<Health>().maxHealth)
                {
                    Player[i].GetComponent<Health>().currentHealth += 3;

                    if (Player[i].GetComponent<Health>().currentHealth > Player[i].GetComponent<Health>().maxHealth)
                        Player[i].GetComponent<Health>().currentHealth = Player[i].GetComponent<Health>().maxHealth;
                }

                HealthBar healthbar = Player[i].GetComponent<Health>().healthBar;
                healthbar.SetHealth(Player[i].GetComponent<Health>().currentHealth);
                
                HadHealth = true;
                
                foreach (GameObject potion in potions)
                {
                    float distplayerpot = Mathf.Abs(potion.transform.position.x - Player[i].transform.position.x);
                    if (distplayerpot < 5)
                        Destroy(potion);   
                }
            }
        }

        if (collider.tag == "speed")
        {
            GameObject[] potions = GameObject.FindGameObjectsWithTag("speed");
            
            if (!HadSpeed)
            {
                for (int i = 0; i < Player.Length; i++)
                {
                    Player[i].GetComponent<Move>().Speed += 60;
                    Player[i].GetComponent<Move>().JumpForce += 100;
                    
                    foreach (GameObject potion in potions)
                    {
                        float distplayerpot = Mathf.Abs(potion.transform.position.x - Player[i].transform.position.x);
                        if (distplayerpot < 5)
                            Destroy(potion);   
                    }
                }

                HadSpeed = true;
            }
            else
            {
                for (int i = 0; i < Player.Length; i++)
                {
                    foreach (GameObject potion in potions)
                    {
                        float distplayerpot = Mathf.Abs(potion.transform.position.x - Player[i].transform.position.x);
                        if (distplayerpot < 5)
                            Destroy(potion);
                    }
                }
            }

            WaitTimeSpeed = 20;            
        }
    }
}
