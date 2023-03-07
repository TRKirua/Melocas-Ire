using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveBoss4S : EnnemyS
{
    public AttackBoss4S attack;

    private bool isOnGround;
    public LayerMask collisionLayers;
    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    
    public float AttackDist;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        PlayerRange = 14f;
        animator.SetBool("idle", true);
    }
    
    
    private void FixedUpdate()
    {
        isOnGround =
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
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
                                mob.localScale = new Vector3(1, 1, 1);
                            else
                                mob.localScale = new Vector3(-1, 1, 1);
                        }
                    }


                    if (distToPlayer > 60f)
                    {
                        animator.SetBool("idle", true);
                        rb.velocity = Vector2.zero;
                    }

                    else
                    {
                        if (distToPlayer > AttackDist || !isOnGround)
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

            if (!isOnGround)
                rb.velocity = new Vector3(rb.velocity.x, -30);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D colliderrr)
    {
        switch (colliderrr.tag)
        {
            case "jump1":
                if (isOnGround)
                {
                    rb.AddForce(new Vector2(40f, 7000));
                    mob.position = new Vector3(mob.position.x - 2, mob.position.y);
                }
                break;
            
            case "jump2":
                if (isOnGround)
                {
                    rb.AddForce(new Vector2(40f, 7000));
                    mob.position = new Vector3(mob.position.x + 2, mob.position.y);
                }
                break;
        }
    }


    public void Attack()
    {
        int n = Random.Range(0, 3);
        rb.velocity = Vector2.zero;
        
        if (n != 2)
            attack.LetsAttack1(basicAttack);
        else
            attack.LetsAttack2(specialAttack);
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
    
    
    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}