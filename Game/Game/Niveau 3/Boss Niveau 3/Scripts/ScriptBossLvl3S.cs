using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class ScriptBossLvl3S : EnnemyS
{
    public AttackBosslvl3S attack;

    public float AttackDist;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        PlayerRange = 14f;
        animator.SetBool("idle", true);
    }


    // Update is called once per frame
    void Update()
    {
        if (isFreeze)
        {
            animator.Rebind();
            animator.Update(0f);
        }
        
        if (state == State.Die)
        {
            rb.velocity = Vector2.zero;
            cd.enabled = false;
            animator.SetTrigger("isDead");
            StartCoroutine(destroyboss());
        }

        else
        {
            rb.velocity = Vector2.zero;
            Transform playerToAttack = listOfPlayers[0].transform;
            
            float distToPlayer = 200;

            foreach (GameObject player in listOfPlayers)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) < distToPlayer)
                {
                    distToPlayer = Mathf.Abs(player.transform.position.x - transform.position.x);
                    playerToAttack = player.transform;
                }
            }

            // Attend le player
            if (state == State.Waiting)
            {
                if (distToPlayer < PlayerRange) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }


            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                if (!isFreeze)
                {
                    if (playerToAttack.position.x < mob.position.x - 0.2f ||
                        playerToAttack.position.x > mob.position.x + 0.2f)
                    {
                        if (!isAttackingEnnemyS)
                        {
                            if (playerToAttack.position.x > mob.position.x)
                                mob.localScale = new Vector3(0.7f, 0.7f, 1);
                            else
                                mob.localScale = new Vector3(-0.7f, 0.7f, 1);
                        }
                    }


                    if (distToPlayer > 60f)
                    {
                        animator.SetBool("idle", true);
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        if (distToPlayer > AttackDist)
                        {
                            animator.SetBool("idle", false);

                            if (!isAttackingEnnemyS)
                                ChasePlayer(playerToAttack); //Chasse le player
                        }

                        else
                        {
                            rb.velocity = Vector2.zero;
                            animator.SetBool("idle", true);
                            Attack();
                        }
                    }
                }
            }
        }
    }

    IEnumerator destroyboss()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }




    public void Attack()
    {
        int n = Random.Range(0, 3);
        rb.velocity = Vector2.zero;
        
        if (n == 0) // Attaque spéciale 1 chance sur 3
        {
           attack.LetsAttack2(specialAttack);
        }
        

        else // Attaque basique 2 chance sur trois
        {
            attack.LetsAttack1(basicAttack);
        }
    }


    // Chasser le joueur
    private void ChasePlayer(Transform player)
    {
        float posX = mob.position.x;
        float posXPlayer = player.position.x;
        
        if (posX < posXPlayer)
            rb.velocity = new Vector2(speed, 0);

        else if (posX > posXPlayer)
            rb.velocity = new Vector2(-speed, 0);
    }
}