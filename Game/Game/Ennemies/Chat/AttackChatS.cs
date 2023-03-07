using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class AttackChatS : MonoBehaviour
{
    public Transform basicAttackPos, specialAttackPos; //Position d'attaque basique
    public float basicAttackRange, specialAttackRange; //Rayon d'attaque basique
    private float currenthealth;
    public LayerMask heros; //Default
    private bool isHit;
    
    private void Start()
    {
        currenthealth = GetComponent<EnnemyS>().health;
    }

    public bool GetHit()
    { 
        isHit = false;
        
        if (GetComponent<EnnemyS>().health != currenthealth)
        {
            currenthealth = GetComponent<EnnemyS>().health;
            isHit = true;
        }

        return isHit;
    }
    
    public void LetsAttack1(int damage)
    {
        isHit = GetHit();

        if (!isHit)
        {
            Collider2D[] heroToDamage =
                Physics2D.OverlapCircleAll(basicAttackPos.position, basicAttackRange, heros);

            for (int j = 0; j < heroToDamage.Length; j++)
            {
                if (heroToDamage[j].GetComponent<DeathPacito>().canhit)
                    heroToDamage[j].GetComponent<Health>().TakeDamage(damage);
            }

            StartCoroutine(SecondTouch(damage));
        }
        
        else
        {
            GetComponent<MoveChatS>().NextAttack = 0;
            GetComponent<MoveChatS>().Attack();
        }
    }


    IEnumerator SecondTouch(int damage)
    {
        yield return new WaitForSeconds(0.31f);
        
        Collider2D[] heroToDamage2 =
            Physics2D.OverlapCircleAll(specialAttackPos.position, specialAttackRange, heros);
        
        for (int j = 0; j < heroToDamage2.Length; j++)
        {
            if (heroToDamage2[j].GetComponent<DeathPacito>().canhit)
                heroToDamage2[j].GetComponent<Health>().TakeDamage(damage);
        }
    }
    
    
    
    public void LetsAttack2(int damage)
    {
        isHit = GetHit();

        if (!isHit)
        {
            Collider2D[] heroToDamage =
                Physics2D.OverlapCircleAll(basicAttackPos.position, specialAttackRange, heros);
        
            for (int i = 0; i < heroToDamage.Length; i++)
            {
                if (heroToDamage[i].GetComponent<DeathPacito>().canhit)
                    heroToDamage[i].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MoveChatS>().NextAttack = 0;
            GetComponent<MoveChatS>().Attack();
        }
    }
    
    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicAttackPos.position, basicAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(specialAttackPos.position, specialAttackRange);
    }
}