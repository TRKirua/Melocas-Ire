using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class JumpMobS : EnnemyS
{

    private float y, x, DistCenterMob, HealthBefore;
    public AttackBrocolosseS attack;
    private Vector3 velocity = Vector3.zero; // Vélocité
    private Vector3 MobPositionBefore, transformPosition;
    private bool isJumping; // Savoir si le joueur est en train de sauter (true ou false)
    public float JumpForce; // Force de saut du joueur (entrée dans unity)
    private bool flip;
    private bool flip2, change = true;
    
    private bool isOnGround, isalreadyonground; // Savoir si le player est au sol (true ou false)
        
    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    public LayerMask collisionLayers;
    public float Scale;
    public float AttackDist, WaitAnimation1, WaitAnimation2;

    public Transform center;

    public float NextAttack;
    public float AttackRate;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        target = waypoints[0];
        length = waypoints.Length;
        PlayerRange = 14f;
        HealthBefore = health;
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
            animator.SetTrigger("isDead");
        
        else
        {
            Transform playerToAttack = listOfPlayers[0].transform;
            
            y = mob.transform.position.y;
            float distToPlayer = 200;

            foreach (GameObject player in listOfPlayers)
            {
                if (Mathf.Abs(player.transform.position.x - center.position.x) < distToPlayer)
                {
                    distToPlayer = Mathf.Abs(player.transform.position.x - center.position.x);
                    playerToAttack = player.transform;
                }
            }


            // Attend le player
            if (state == State.Waiting)
            {
                float DistCenterMob = mob.position.x - center.position.x;

                float distToWaypoint =
                    Vector3.Distance(center.position, target.position); //Distance au prochain point

                Vector3 dir = target.position - center.position;
                mob.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

                if (distToWaypoint < 0.5f)
                {
                    destPoint = (destPoint + 1) % length; //Prochain waypoint
                    target = waypoints[destPoint]; //Cible
                    
                    graphics.transform.localScale = new Vector3(-graphics.transform.localScale.x, Scale);
                    graphics.transform.position =
                        new Vector3(waypoints[(destPoint + 1) % length].position.x -  DistCenterMob, graphics.transform.position.y);
                }

                if (distToPlayer < PlayerRange) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }


            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                MobPositionBefore = mob.position;

                if (!isOnGround)
                    isJumping = true;

                else
                {
                    if (!isFreeze)
                    {
                        DistCenterMob = mob.position.x - center.position.x;

                        for (float i = 0; i < 1; i += 0.01f)
                        {
                            if (MobPositionBefore.x == mob.position.x + i)
                            {
                                MobPositionBefore = mob.position;
                                isJumping = true;
                            }
                        }

                        for (float i = 0; i < 1; i += 0.01f)
                        {
                            if (MobPositionBefore.x == mob.position.x - i)
                            {
                                MobPositionBefore = mob.position;
                                isJumping = true;
                            }
                        }


                        if (playerToAttack.position.x > center.position.x)
                        {
                            if (mob.localScale.x < 0)
                            {
                                if (change)
                                {
                                    graphics.transform.localScale =
                                        new Vector3(-graphics.transform.localScale.x, Scale);
                                    graphics.transform.position = new Vector3(
                                        mob.position.x + Math.Abs(DistCenterMob * 2),
                                        graphics.transform.position.y);
                                    change = false;
                                }
                            }
                        }

                        else
                        {
                            if (mob.localScale.x > 0)
                            {
                                graphics.transform.localScale = new Vector3(-Scale, Scale);

                                graphics.transform.position = new Vector3(mob.position.x - Math.Abs(DistCenterMob * 2),
                                    graphics.transform.position.y);
                            }

                            change = true;
                        }

                        if (distToPlayer > 15f)
                        {
                            animator.SetBool("idle", true);
                            rb.velocity = Vector2.zero;
                        }

                        else
                        {
                            if (distToPlayer > AttackDist)
                            {
                                animator.SetBool("idle", false);
                                ChasePlayer(playerToAttack); //Chasse le player
                            }
                            else
                                Attack();
                        }
                    }
                }
            }
        }
    }
    
    public void Attack()
    {
        if (health > 0)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("idle", true);

            if (Time.time >= NextAttack)
            {
                NextAttack = Time.time + 1f / AttackRate;
                int n = Random.Range(0, 3);
                if (n != 2)
                {
                    animator.SetTrigger("attack1");
                    StartCoroutine(Waitanimation1());
                }
                else
                {
                    animator.SetTrigger("attack2");
                    StartCoroutine(Waitanimation2());
                }
            }
        }
    }
    
    IEnumerator Waitanimation1()
    {
        yield return new WaitForSeconds(WaitAnimation1);
        attack.LetsAttack1(basicAttack);
    }

    IEnumerator Waitanimation2()
    {
        yield return new WaitForSeconds(WaitAnimation2);
        attack.LetsAttack2(specialAttack);
    }
    
    // Chasser le joueur
    private void ChasePlayer(Transform player)
    {
        float posX = center.position.x;
        float posXPlayer = player.position.x;
        float DistMobPlayer = Mathf.Abs(player.transform.position.y - mob.transform.position.y);

        if (DistMobPlayer < 10)
        {
            MovePlayer(speed);

            if (posX < posXPlayer)
                rb.velocity = new Vector2(speed, 0);

            else if (posX > posXPlayer)
                rb.velocity = new Vector2(-speed, 0);
        }

    }
    
    void MovePlayer(float _horizontalMovement)
    {
        Vector3 target = new Vector2(_horizontalMovement, rb.velocity.y); // Calculer la vélocité du player
        rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref velocity, .05f); // Effectuer le mouvement

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, JumpForce)); // Saut
            isJumping = false; // isJumping est remis à faux
        }
    }
}
