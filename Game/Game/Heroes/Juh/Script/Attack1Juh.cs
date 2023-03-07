using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Numerics;
using Niveau_1.Boss_lv1.Scripts;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using ScriptPlayer;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public class Attack1Juh : MonoBehaviour
{
    public int BasicAttack = 2; // Puissance attaque basique

    private float bAttackRate = 2f; //Fréquence d'attaque basique
    private float bNextAttack; //Prochaine attaque basique
    public Transform basicAttackPos; //Position d'attaque basique
    public float basicAttackRange; //Rayon d'attaque basique

    public LayerMask ennemies; //Ennemis
    public Animator animator; //Animation joueur
    public bool isAttacking1Juh;

    public Color FreezeColor; //Couleur mob freeze


    void Update()
    {
        if (GetComponent<Move>().isNotDead && GetComponent<Move>().isNotHit)
        {
            if (!GetComponent<Attack2Juh>().isAttacking2Juh && !isAttacking1Juh)
            {
                if (Time.time >= bNextAttack)
                {
                    if (Input.GetButtonDown("Fire1")) // Si le joueur active l'attaque basique
                    {
                        bNextAttack = Time.time + 1f / bAttackRate;
                        isAttacking1Juh = true;
                        LetsAttackBasic(BasicAttack);
                    }
                }
            }
        }
    }
    

    
    void LetsAttackBasic(int damage)
    {
        animator.SetTrigger("attack1"); //Animation attaque basique
        Collider2D[] ennemiesToDamage = Physics2D.OverlapCircleAll(basicAttackPos.position, basicAttackRange, ennemies);
        //Ceux qui sont dans le champ d'attaque
        
        int n = Random.Range(0, 5);
        bool b = n < 1;

        if (b)
        {
            for (int i = 0; i < ennemiesToDamage.Length; i++)
            {
                if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
                {
                    ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(BasicAttack);

                    if (!ennemiesToDamage[i].GetComponent<EnnemyS>().isFreeze && ennemiesToDamage[i].GetComponent<EnnemyS>().health > BasicAttack)
                    {
                        var color = ennemiesToDamage[i].GetComponent<SpriteRenderer>().material.color;
                        ennemiesToDamage[i].GetComponent<SpriteRenderer>().material.color = FreezeColor;
                        ennemiesToDamage[i].GetComponent<EnnemyS>().isFreeze = true;

                        StartCoroutine(Defreeze(ennemiesToDamage[i], color));
                    }
                }
            }
        }

        else
            for (int i = 0; i < ennemiesToDamage.Length; i++)
                if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
                    ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(BasicAttack);

        StartCoroutine(WaitForAnimation());
    }

    
    
    IEnumerator Defreeze(Collider2D mob, Color color)
    {
        yield return new WaitForSeconds(1.5f);
        mob.GetComponent<SpriteRenderer>().material.color = color;
        mob.GetComponent<EnnemyS>().isFreeze = false;
    }
    
    
    
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking1Juh = false;
    }

    
    
    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicAttackPos.position, basicAttackRange);
    }
}