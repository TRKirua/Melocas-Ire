using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Numerics;
using Niveau_1.Boss_lv1.Scripts;
using ScriptPlayer;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Attack1Otema : MonoBehaviour
{
    public int BasicAttack = 4; // Puissance attaque basique

    private float bAttackRate = 0.8f; //Fréquence d'attaque basique
    private float bNextAttack; //Prochaine attaque basique
    public Transform basicAttackPos; //Position d'attaque basique
    public Vector2 basicAttackRange; //Rayon d'attaque basique

    public LayerMask ennemies; //Ennemis

    public Animator animator; //Animation joueur
    public bool isAttacking1Otema;

    public GameObject Explosion; //explosion
    private bool explosionAppear; //faire apparaitre l'explosion
    private int critiqueCount; //nb de coups pr l'attaque critique
    
    
    private void Start()
    {
        critiqueCount = 0;
    }

    
    
    void Update()
    {
        if (GetComponent<Move>().isNotDead && GetComponent<Move>().isNotHit)
        {
            if (!GetComponent<Attack2Otema>().isAttacking2Otema && !isAttacking1Otema)
            {
                if (Time.time >= bNextAttack)
                {
                    if (Input.GetButtonDown("Fire1")) // Si le joueur active l'attaque basique
                    {
                        bNextAttack = Time.time + 1f / bAttackRate;
                        isAttacking1Otema = true;
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

        if (critiqueCount == 4)
        {
            damage += 2;
            critiqueCount = 0;
            explosionAppear = true;
        }
        else if (ennemiesToDamage.Length > 0)
            critiqueCount += 1;

        
        for (int i = 0; i < ennemiesToDamage.Length; i++)
        {
            if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
            {
                ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(damage);
                if (explosionAppear)
                {
                    Instantiate(Explosion, basicAttackPos);
                    explosionAppear = false;
                }
            }
        }

        StartCoroutine(WaitForAnimation());
    }


    
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking1Otema = false;
    }

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(basicAttackPos.position, basicAttackRange);
    }
}