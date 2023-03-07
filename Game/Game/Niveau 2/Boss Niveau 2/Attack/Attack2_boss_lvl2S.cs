using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2_boss_lvl2S : MonoBehaviour
{
    private float lifetime = 1f; //Temps de vie
    private float n;
    private bool canTouch;

    private void FixedUpdate()
    {
        n += 0.02f;
        
        if (n > lifetime)
            Destroy(gameObject);

        if (!canTouch)
        {
            if (n > 0.16f)
                canTouch = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D hitbox)
    {
        if (canTouch)
        {
            Health player = hitbox.GetComponent<Health>();

            if (player != null)
                player.TakeDamage(3);
        }
    }
}