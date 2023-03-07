using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobDarkCerbalS : EnnemyS
{
    private float timer;
    private float WaitTime = 5.0f;
    private float WaitTime2 = 1f;

    private float y, DistCenterMob;
    public AttackDarkCerbalS attack;
    private bool flip;
    private bool flip2, change = true;

    private bool isOnGround; // Savoir si le player est au sol (true ou false)

    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    public LayerMask collisionLayers;
    public float Scale;
    public float AttackDist, WaitAnimation1, WaitAnimation2;

    public float NextAttack;
    public float AttackRate; //Fréquence d'attaque basique
    public bool isAttacking, isTeleporting, isDead;
    public bool alreadyattacked = true;
    
    public Transform center;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        target = waypoints[1];
        length = waypoints.Length;
        PlayerRange = 17f;
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
            isDead = true;
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
                rb.velocity = Vector2.zero;

                timer += Time.deltaTime;
                    if (timer > WaitTime)
                {
                    timer = 0f;
                    animator.SetTrigger("Teleport");
                    StartCoroutine(WaitForTeleport());
                }

                if (distToPlayer < PlayerRange) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }


            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                if (!isOnGround)
                {
                    animator.SetTrigger("Teleport");
                    StartCoroutine(WaitTeleportVoid(playerToAttack));
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

                        if (distToPlayer > AttackDist)
                        {
                            if (!isAttacking)
                                ChasePlayer(playerToAttack); //Chasse le player
                        }

                        else
                        {
                            if (GetComponent<AttackDarkCerbalS>().GetHit())
                            {
                                alreadyattacked = false;
                                isTeleporting = true;
                                animator.SetTrigger("Teleport");
                                StartCoroutine(GetComponent<AttackDarkCerbalS>().WaitForTeleport(playerToAttack));
                            }
                            
                            else
                            {
                                if (!isTeleporting)
                                    if (alreadyattacked)
                                        Attack();
                            }
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
    
    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(0.5f);
        float TeleportPosition = Random.Range(waypoints[0].position.x, waypoints[1].position.x);
        mob.transform.position = new Vector3(TeleportPosition, waypoints[0].position.y);
        
        isTeleporting = false;
    }

    // Chasser le joueur
    private void ChasePlayer(Transform player)
    {
        float DistMobPlayer = Mathf.Abs(player.transform.position.x - mob.transform.position.x);

        if (DistMobPlayer < 17)
        {
            timer += Time.deltaTime;
            if (timer > WaitTime2)
            {
                timer = 0f;
                animator.SetTrigger("Teleport");
                isTeleporting = true;
                StartCoroutine(WaitTeleportChase(player));
            }
        }
        else
            state = State.Waiting;
    }

    IEnumerator WaitTeleportVoid(Transform player)
    {
        yield return new WaitForSeconds(0.5f);

        timer += Time.deltaTime;
        if (timer > WaitTime2)
        {
            timer = 0f;
            int random = Random.Range(0, 2);
            if (random == 1)
                mob.transform.position =
                    new Vector2(player.position.x + 4, player.position.y);
            else
                mob.transform.position =
                    new Vector2(player.position.x - 4, player.position.y);
        }
        
        isTeleporting = false;
    }

    IEnumerator WaitTeleportChase(Transform player)
    {
        yield return new WaitForSeconds(0.5f);

        float DistMobPlayer = Mathf.Abs(player.transform.position.x - mob.transform.position.x);
        float posX = center.position.x;
        float posXPlayer = player.position.x;
        float posY = center.position.y;
        float posYPlayer = player.position.y;

        if (DistMobPlayer < 17 && DistMobPlayer > 15)
        {
            if (posY < posYPlayer)
            {
                if (posX < posXPlayer)
                    mob.transform.position =
                        new Vector2(mob.transform.position.x + DistMobPlayer / 2, posYPlayer);

                else if (posX > posXPlayer)
                    mob.transform.position =
                        new Vector2(mob.transform.position.x - DistMobPlayer / 2, posYPlayer);
            }
            
            else
            {
                if (posX < posXPlayer)
                    mob.transform.position =
                        new Vector2(mob.transform.position.x + DistMobPlayer / 2, mob.transform.position.y);

                else if (posX > posXPlayer)
                    mob.transform.position =
                        new Vector2(mob.transform.position.x - DistMobPlayer / 2, mob.transform.position.y);
            }
        }
        
        else
        {
            if (DistMobPlayer > 9)
            {
                if (posY < posYPlayer)
                {
                    if (posX < posXPlayer)
                        mob.transform.position =
                            new Vector2(mob.transform.position.x + DistMobPlayer / 3, posYPlayer);

                    else if (posX > posXPlayer)
                        mob.transform.position =
                            new Vector2(mob.transform.position.x - DistMobPlayer / 3, posYPlayer);
                }
                
                else
                {
                    if (posX < posXPlayer)
                        mob.transform.position =
                            new Vector2(mob.transform.position.x + DistMobPlayer / 3, mob.transform.position.y);

                    else if (posX > posXPlayer)
                        mob.transform.position =
                            new Vector2(mob.transform.position.x - DistMobPlayer / 3, mob.transform.position.y);
                }
            }
            else
            {
                int random = Random.Range(0, 2);
                if (random == 1)
                {
                    if (posY < posYPlayer)
                        mob.transform.position = new Vector2(player.position.x + 4f, posYPlayer);
                    else
                        mob.transform.position = new Vector2(player.position.x + 4f, mob.transform.position.y);
                }
                else
                {
                    if (posY < posYPlayer)
                        mob.transform.position = new Vector2(player.position.x - 4f, posYPlayer);
                    else
                        mob.transform.position = new Vector2(player.position.x - 4f, mob.transform.position.y);
                }
                   
            }
        }
        
        isTeleporting = false;
    }
}
