using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobNightBorneS : EnnemyS
{
    private float y,  DistCenterMob;
    public AttackNightBorneS attack;
    private bool flip;
    private bool flip2, change = true;
    public float middlelife;
    private bool isOnGround, phase2;
        
    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    public LayerMask collisionLayers;
    public float HeightDie, JumpForce;
    public float Scale, AttackDist, WaitAnimation1, WaitAnimation2;
    
    public Transform center;
    public bool isAttacking, isDead;
    
    private Color newcolor;
    public Renderer renderer;

    public float NextAttack;
    public float AttackRate, InitialAttackRate, InitialSpeed; //Fréquence d'attaque basique
    public bool alreadyattacked = true;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        target = waypoints[1];
        length = waypoints.Length;
        PlayerRange = 14f;
        y = mob.transform.position.y;
        
        newcolor = renderer.material.color;
        InitialAttackRate = AttackRate;
        InitialSpeed = speed;
        
        if (mob.tag == "blueNightBorn")
        {
            if (ColorUtility.TryParseHtmlString("#0083FF", out newcolor))
                renderer.material.color = newcolor;
        }
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
        
        if (health <= middlelife)
        {
            if (!phase2)
            {
                speed *= 2;
                basicAttack += 1;
                specialAttack += 1;
                JumpForce /= 2;
                AttackRate *= 2;

                if (ColorUtility.TryParseHtmlString("#FFFFFF", out newcolor))
                    renderer.material.color = newcolor;

                if (mob.tag == "blueNightBorn")
                {
                    if (ColorUtility.TryParseHtmlString("#FFFC00", out newcolor))
                        renderer.material.color = newcolor;
                }
                else
                {
                    if (ColorUtility.TryParseHtmlString("#FF0000", out newcolor))
                        renderer.material.color = newcolor;
                }


                phase2 = true;
            }
        }
        
        if (state == State.Die)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            cd.enabled = false;
            animator.SetTrigger("isDead");
            
            var transformPosition = mob.transform.position;
            transformPosition.y = y;
            mob.transform.position = transformPosition;

            StartCoroutine(WaitDestroy()); 
        }

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

                if (distToWaypoint < 2f)
                {
                    destPoint = (destPoint + 1) % length; //Prochain waypoint
                    target = waypoints[destPoint]; //Cible
                    
                    graphics.transform.localScale = new Vector3(-graphics.transform.localScale.x, Scale);
                    graphics.transform.position =
                        new Vector3(waypoints[(destPoint + 1) % length].position.x -  DistCenterMob, waypoints[(destPoint + 1) % length].position.y);
                }

                if (distToPlayer < PlayerRange) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }


            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                if (!isOnGround)
                {
                    if (mob.position.y <= HeightDie)
                    {
                      Destroy(gameObject);
                    }
                }
                
                else
                {
                    if (!isFreeze)
                    {
                        DistCenterMob = mob.position.x - center.position.x;

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
                            {
                                if (GetComponent<AttackNightBorneS>().GetHit())
                                {
                                    alreadyattacked = false;
                                    AttackRate = 1;
                                    StartCoroutine(WaitAttackAgain());

                                    float dist = Mathf.Abs(playerToAttack.position.x - mob.position.x);
                                    if (dist < 5)
                                    {
                                        float posX = center.position.x;
                                        float posXPlayer = playerToAttack.position.x;
                                        if (posXPlayer < posX)
                                            rb.AddForce(new Vector2(1000, JumpForce));
                                        else
                                            rb.AddForce(new Vector2(-1000, JumpForce));

                                        speed += 10;
                                    }
                                }

                                if (alreadyattacked)
                                {
                                    speed = InitialSpeed;
                                    Attack();
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator WaitAttackAgain()
    {
        yield return new WaitForSeconds(1);
        alreadyattacked = true;
        AttackRate = InitialAttackRate;
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
                    isAttacking = true;
                    StartCoroutine(Waitanimation1());
                }
                else
                {
                    animator.SetTrigger("attack2");
                    isAttacking = true;
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

    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    
    // Chasser le joueur
    private void ChasePlayer(Transform player)
    {
        if (!isAttacking)
        {
            float posX = center.position.x;
            float posXPlayer = player.position.x;

            if (posX < posXPlayer)
                rb.velocity = new Vector2(speed, rb.velocity.y);

            else if (posX > posXPlayer)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "collider":
                rb.AddForce(new Vector2(10f, JumpForce));
                break;
        }
    }
}
