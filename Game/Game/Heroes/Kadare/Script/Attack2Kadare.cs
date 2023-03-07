using System.Collections;
using System.Collections.Generic;
using ScriptPlayer;
using UnityEngine;

public class Attack2Kadare : MonoBehaviour
{
    private float mAttackRate = 0.15f; //Fréquence d'attaque magique
    private float mNextAttack; //Prochaine attaque magique
    
    public GameObject magicProjectile; //Projectile
    public Transform appearPoint; //Point d'apparition du projectile

    public Animator animator; //Animation joueur

    public bool isAttacking2Kadare;


    
    void Update()
    {
        if (transform.localScale.x > 0)
        {
            Quaternion q = appearPoint.rotation;
            q.y = 180;
            appearPoint.rotation = q;
        }
        
        else
        {
            Quaternion q = appearPoint.rotation;
            q.y = 0;
            appearPoint.rotation = q;
        }

        if (GetComponent<Move>().isNotDead && GetComponent<Move>().isNotHit)
        {
            if (!GetComponent<Attack1Kadare>().isAttacking1Kadare && !isAttacking2Kadare &&
                !GetComponent<PassifKadare>().isPassif)
            {
                if (Time.time >= mNextAttack)
                {
                    if (Input.GetButtonDown("Fire2")) // Si le joueur active l'attaque magique
                    {
                        transform.GetComponent<Move>().isAttacking2 = true;
                        mNextAttack = Time.time + 1f / mAttackRate;
                        isAttacking2Kadare = true;
                        LetsAttackMagic();
                    }
                }
            }
        }
    }
    
    
    
    void LetsAttackMagic()
    {
        animator.SetTrigger("attack2");
        Instantiate(magicProjectile, appearPoint.position, appearPoint.rotation);
        
        StartCoroutine(WaitForAnimation());
    }
    
    
    
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.85f);
        isAttacking2Kadare = false;
        transform.GetComponent<Move>().isAttacking2 = false;
    }
}