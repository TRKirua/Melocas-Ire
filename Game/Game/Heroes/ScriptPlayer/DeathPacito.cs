using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using ScriptPlayer;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class DeathPacito : MonoBehaviour
{
    public Health hl; //Vie du joueur
    public Rigidbody2D rb; //Rigidbody du joueur
    public bool canhit = true;
    public Animator animator; //Animation
    public float height; //Hauteur d'activation de la reap

    public bool isDead;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= height)
        { 
            transform.position = new Vector2(-2, 1);
            rb.velocity = Vector2.zero;
            transform.GetComponent<Health>().currentHealth = transform.GetComponent<Health>().maxHealth;
            transform.GetComponent<Health>().healthBar.SetHealth(transform.GetComponent<Health>().currentHealth);
            isDead = true;
        }

        if (hl.GetComponent<Health>().currentHealth < 1)
        { 
            StartCoroutine(WaitForAnimation());
            transform.GetComponent<Health>().currentHealth = transform.GetComponent<Health>().maxHealth;
            canhit = false;
            transform.GetComponent<Move>().isNotDead = false;
            rb.velocity = Vector2.zero;
            isDead = true;
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(1.9f);
        transform.position = new Vector2(-2, 1);
        animator.SetTrigger("reap");
        transform.GetComponent<Health>().healthBar.SetHealth(transform.GetComponent<Health>().currentHealth);
        canhit = true;
        transform.GetComponent<Move>().isNotDead = true;
    }

}