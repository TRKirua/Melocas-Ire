using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveSiamoisS : EnnemyS
{
    private float y;
    public AttackSiamoisS attack;

    private bool isOnGround; // Savoir si le player est au sol (true ou false)
        
    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    public LayerMask collisionLayers;
    public float HeightDie;
    public float Scale;
    public float AttackDist;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        target = waypoints[1];
        length = waypoints.Length;
        PlayerRange = 14f;
        y = mob.transform.position.y;
    }

    private void FixedUpdate()
    {
        isOnGround =
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,
                collisionLayers);
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
            
            var transformPosition = mob.transform.position;
            transformPosition.y = y;
            mob.transform.position = transformPosition;
        }

        else
        {
            Transform playerToAttack = listOfPlayers[0].transform;
            
            y = mob.transform.position.y;
            float distToPlayerX = 200;
            float distToPlayerY = 0;

            foreach (GameObject player in listOfPlayers)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) < distToPlayerX)
                {
                    distToPlayerX = Mathf.Abs(player.transform.position.x - transform.position.x);
                    distToPlayerY = Mathf.Abs(player.transform.position.y - transform.position.y);
                    playerToAttack = player.transform;
                }
            }
            
            // Attend le player
            if (state == State.Waiting)
            {
                float distToWaypoint =
                    Vector3.Distance(mob.position, target.position); //Distance au prochain point

                Vector3 dir = target.position - mob.position;
                mob.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

                if (distToWaypoint < 0.5f)
                {
                    destPoint = (destPoint + 1) % length; //Prochain waypoint
                    target = waypoints[destPoint]; //Cible
                    
                    graphics.transform.localScale = new Vector3(-graphics.transform.localScale.x, Scale);
                }

                if (distToPlayerX < PlayerRange && distToPlayerY < 2) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }


            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                if (!isOnGround)
                {
                    rb.velocity = new Vector2(0, -speed*2);

                    if (mob.position.y <= HeightDie)
                        Destroy(gameObject);
                }

                else
                {
                    if (!isFreeze)
                    {
                        if (playerToAttack.position.x > mob.position.x + 0.2f)
                        {
                            if (mob.localScale.x < 0)
                                mob.localScale = new Vector3(-mob.localScale.x, mob.localScale.y);
                        }

                        else if (playerToAttack.position.x < mob.position.x - 0.2f)
                            if (mob.localScale.x > 0)
                                mob.localScale = new Vector3(-mob.localScale.x, mob.localScale.y);


                        if (distToPlayerX > 15 || distToPlayerY > 9)
                        {
                            rb.velocity = Vector2.zero;
                            animator.SetBool("idle", true);
                        }

                        else
                        {
                            if (distToPlayerX > AttackDist + 1.5f)
                            {
                                animator.SetBool("idle", false);
                                ChasePlayer(playerToAttack); //Chasse le player
                            }

                            else
                            {
                                rb.velocity = Vector2.zero;
                                animator.SetBool("idle", true);

                                if (distToPlayerX < 1.2f)
                                    attack.LetsAttack2(specialAttack);
                                else
                                    attack.LetsAttack1(basicAttack);
                            }
                        }
                    }
                }
            }
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
