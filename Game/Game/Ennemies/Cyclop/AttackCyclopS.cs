﻿using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEditor;
using UnityEngine;

public class AttackCyclopS : MonoBehaviour
{
    public float AttackRate; //Fréquence d'attaque
    private float NextAttack;

    public Transform basicAttackPos, specialAttackPos, specialAttackPos2; //Position d'attaque
    public float basicAttackRange; //Rayon d'attaque basique

    public float WaitAnimation1, WaitAnimation2;

    public Animator animator; //Animator
    public LayerMask heros; //Default
    

    public void LetsAttack1(int damage)
    {
        if (Time.time >= NextAttack)
        {
            NextAttack = Time.time + 1f / AttackRate;

            animator.SetTrigger("attack1");
            StartCoroutine(WaitForAnimation1(damage));
        }
    }
    
    IEnumerator WaitForAnimation1(int damage)
    {
        yield return new WaitForSeconds(WaitAnimation1);
        
        Collider2D[] heroToDamage =
            Physics2D.OverlapCircleAll(basicAttackPos.position, basicAttackRange, heros);

        for (int j = 0; j < heroToDamage.Length; j++)
        {
            if (heroToDamage[j].GetComponent<DeathPacito>().canhit)
                heroToDamage[j].GetComponent<Health>().TakeDamage(damage);
        }
    }

    public void LetsAttack2(int damage)
    {
        if (Time.time >= NextAttack)
        {
            NextAttack = Time.time + 1f / AttackRate;

            animator.SetTrigger("attack2");
            StartCoroutine(WaitForAnimation2(damage));
        }
    }

    IEnumerator WaitForAnimation2(int damage)
    {
        yield return new WaitForSeconds(WaitAnimation2);
        
        Collider2D[] heroToDamage =
            Physics2D.OverlapAreaAll(specialAttackPos.position, specialAttackPos2.position, heros);
        
        for (int i = 0; i < heroToDamage.Length; i++)
        {
            if (heroToDamage[i].GetComponent<DeathPacito>().canhit)
                heroToDamage[i].GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicAttackPos.position, basicAttackRange);
    }
}