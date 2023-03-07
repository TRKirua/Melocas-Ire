using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class AttackFrigoS : MonoBehaviour
{
    public Transform basicAttackPos, specialAttackPos; //Position d'attaque basique
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
                Physics2D.OverlapAreaAll(basicAttackPos.position, specialAttackPos.position, heros);

            for (int j = 0; j < heroToDamage.Length; j++)
            {
                if (heroToDamage[j].GetComponent<DeathPacito>().canhit)
                    heroToDamage[j].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MobFrigoS>().NextAttack = 0;
            GetComponent<MobFrigoS>().Attack();
        }
    }

    public void LetsAttack2(int damage)
    {
        isHit = GetHit();

        if (!isHit)
        {
            Collider2D[] heroToDamage =
                Physics2D.OverlapAreaAll(basicAttackPos.position, specialAttackPos.position, heros);
        
            for (int i = 0; i < heroToDamage.Length; i++)
            {
                if (heroToDamage[i].GetComponent<DeathPacito>().canhit)
                    heroToDamage[i].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MobFrigoS>().NextAttack = 0;
            GetComponent<MobFrigoS>().Attack();
        }
    }
}