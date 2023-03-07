using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Numerics;
using Niveau_1.Boss_lv1.Scripts;
using ScriptPlayer;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Attack1Kadare : MonoBehaviour
{
    public int BasicAttack = 4; // Puissance attaque basique

    private float bAttackRate = 0.9f; //Fréquence d'attaque basique
    private float bNextAttack; //Prochaine attaque basique
    public Transform basicAttackPos; //Position d'attaque basique
    public Vector2 basicAttackRange; //Rayon d'attaque basique

    public LayerMask ennemies; //Ennemis

    public Animator animator; //Animation joueur
    public bool isAttacking1Kadare;
    
    
    
    void Update()
    {
        if (GetComponent<Move>().isNotDead && GetComponent<Move>().isNotHit)
        {
            if (!GetComponent<Attack2Kadare>().isAttacking2Kadare && !isAttacking1Kadare &&
                !GetComponent<PassifKadare>().isPassif)
            {
                if (Time.time >= bNextAttack)
                {
                    if (Input.GetButtonDown("Fire1")) // Si le joueur active l'attaque basique
                    {
                        bNextAttack = Time.time + 1f / bAttackRate;
                        isAttacking1Kadare = true;
                        LetsAttackBasic(BasicAttack);
                    }
                }
            }
        }
    }



    void LetsAttackBasic(int damage)
    {
        animator.SetTrigger("attack1"); //Animation attaque basique
        Collider2D[] ennemiesToDamage = Physics2D.OverlapBoxAll(basicAttackPos.position, basicAttackRange, 90, ennemies);
        //Ceux qui sont dans le champ d'attaque

        for (int i = 0; i < ennemiesToDamage.Length; i++)
        {
            if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
            {
                ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(BasicAttack);
            }
        }

        StartCoroutine(WaitForAnimation());
    }



    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking1Kadare = false;
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(basicAttackPos.position, basicAttackRange);
    }
}
