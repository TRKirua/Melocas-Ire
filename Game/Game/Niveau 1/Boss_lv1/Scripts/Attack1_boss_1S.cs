using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class Attack1_boss_1S : MonoBehaviour
{
    public Rigidbody2D rb; //Rigidbody projectile
    private float speed = 30; //Vitesse projectile
    private int lifetime = 2; //Temps de vie
    private float n;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void FixedUpdate()
    {
        n += 0.01f;
        
        if (n > lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hitbox)
    {
        Health player = hitbox.GetComponent<Health>();

        if (player != null)
        {
            player.TakeDamage(2);
            Destroy(gameObject);
        }
    }
}