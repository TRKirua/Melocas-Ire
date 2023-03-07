using System;
using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveDarkJuhS : EnnemyS
{
    private float timer;
    private float WaitTime = 5.0f;
    private float WaitTime2 = 1f;
    private float y, DistCenterMob;
    public AttackDarkJuhS attack;
    private bool flip;
    public bool isTeleporting;
    private bool phase2;
    private bool flip2, change = true;
    private bool isOnGround; // Savoir si le player est au sol (true ou false)
    public Transform groundCheck; // Vérification du contact avec le sol
    public float groundCheckRadius; // Rayon du cercle de vérification
    public LayerMask collisionLayers;
    public float Scale;
    public float AttackDist, WaitAnimation1, WaitAnimation2, WaitAnimation3;
    public float NextAttack, middlelife;
    public float AttackRate; //Fréquence d'attaque basique
    public int thirdAttack;
    public bool isAttacking;
    public bool alreadyattacked = true;
    private Color newcolor, oldcolor;
    public Renderer renderer;
    public Transform center;
    private float oldspeed, oldattackrate;

    public int ChanceToGenerate; //Chance de généré un clone
    public Transform[] prefabmob;

    private Transform playerToAttack;

    void Start()
    {
        length = waypoints.Length;
        PlayerRange = 17f;
        y = mob.transform.position.y;

        oldcolor = renderer.material.color;

        oldspeed = speed;
        oldattackrate = AttackRate;
    }

    private void FixedUpdate()
    {
        isOnGround =
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,
                collisionLayers);
    }

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
                if (ColorUtility.TryParseHtmlString("#FF0000", out newcolor))
                    renderer.material.color = newcolor;
                AttackRate = 0.6f;
                basicAttack += 1;
                speed += 3;
                specialAttack += 1;
                thirdAttack += 1;
                AttackDist = 3;
                ChanceToGenerate = 7;
                
                phase2 = true;
            }
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
                    isTeleporting = true;
                    StartCoroutine(WaitTeleportVoid());
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
                                    graphics.transform.localScale = new Vector3(-graphics.transform.localScale.x, Scale);
                                    graphics.transform.position = new Vector3(mob.position.x + Math.Abs(DistCenterMob * 2), graphics.transform.position.y);
                                    change = false;
                                }
                            }
                        }
                    
                        else
                        {
                            if (mob.localScale.x > 0)
                            {
                                graphics.transform.localScale = new Vector3(-Scale, Scale);
                                graphics.transform.position = new Vector3(mob.position.x - Math.Abs(DistCenterMob * 2), graphics.transform.position.y);
                            }
                            change = true;
                        }
                    
                        if (distToPlayer > AttackDist)
                        {
                            if (!isAttacking)
                                ChasePlayer(); //Chasse le player
                        }
                        
                        else
                        {
                            if (GetComponent<AttackDarkJuhS>().GetHit())
                            {
                                alreadyattacked = false;
                                isTeleporting = true;
                            
                                float n = Random.Range(0f, 9f);
                                if (n < ChanceToGenerate)
                                {
                                    int n1 = (int) Random.Range(0f, prefabmob.Length - 1);
                                    int i = 0;
                                    while (i < prefabmob.Length)
                                    {
                                        if (n1 == i)
                                        {
                                            Transform clone = Instantiate(prefabmob[i], GetComponent<MoveDarkJuhS>().mob.position, GetComponent<MoveDarkJuhS>().mob.rotation);
                                        
                                            clone.GetComponent<EnnemyS>().camAnim = camAnim;
                                            clone.GetComponent<EnnemyS>().waypoints[0] = waypoints[0];
                                            clone.GetComponent<EnnemyS>().waypoints[1] = waypoints[1];
                                            clone.GetComponent<EnnemyS>().listOfPlayers = listOfPlayers;
                                        }
                                        
                                        i += 1;
                                    }
                                }
                            
                                animator.SetTrigger("Teleport");
                            }
                        
                            if (!isTeleporting)
                                Attack(playerToAttack);
                        }
                    }
                }
            }
        }
    }

    public void Attack(Transform player)
    {
        if (health > 0)
        {
            rb.velocity = Vector2.zero;
            
            if (Time.time >= NextAttack)
            {
                float distToPlayer = Mathf.Abs(playerToAttack.position.x - center.position.x);
                NextAttack = Time.time + 1f / AttackRate;
                if (health > middlelife)
                {
                    if (distToPlayer < 4)
                    {
                        int n = Random.Range(2, 4);
                        if (n != 2)
                        {
                            animator.SetTrigger("attack2");
                            isAttacking = true;
                            StartCoroutine(Waitanimation2(player));
                        }
                        else
                        {
                            float posX = center.position.x;
                            float posXPlayer = playerToAttack.position.x;
                            if (posX < posXPlayer)
                                mob.transform.position =
                                    new Vector2(mob.transform.position.x - 3, mob.transform.position.y);
                            else
                            {
                                mob.transform.position =
                                    new Vector2(mob.transform.position.x + 3, mob.transform.position.y);
                            }
                            
                            animator.SetTrigger("attack3");
                            isAttacking = true;
                            StartCoroutine(Waitanimation3(player));
                        }
                    }
                    else
                    {
                        animator.SetTrigger("attack1");
                        isAttacking = true;
                        StartCoroutine(Waitanimation1(player));
                    }
                }
                else
                {
                    int n = Random.Range(2, 4);
                    if (n != 2)
                    {
                        renderer.material.color = oldcolor;
                        if (ColorUtility.TryParseHtmlString("#EAFF00", out newcolor))
                            renderer.material.color = newcolor;
                        speed = oldspeed;
                        AttackRate = 1;
                        
                        animator.SetTrigger("attack2");
                        isAttacking = true;
                        StartCoroutine(Waitanimation2(player));
                    }
                    else
                    {
                        float posX = center.position.x;
                        float posXPlayer = playerToAttack.position.x;
                        if (posX < posXPlayer)
                            mob.transform.position =
                                new Vector2(mob.transform.position.x - 3, mob.transform.position.y);
                        else
                        {
                            mob.transform.position =
                                new Vector2(mob.transform.position.x + 3, mob.transform.position.y);
                        }
                        renderer.material.color = oldcolor;
                        if (ColorUtility.TryParseHtmlString("#00FFFE", out newcolor))
                            renderer.material.color = newcolor;
                        speed = 20;
                        AttackRate = oldattackrate;
                        
                        animator.SetTrigger("attack3");
                        isAttacking = true;
                        StartCoroutine(Waitanimation3(player));
                    }
                }
            }
        }
    }


    IEnumerator Waitanimation1(Transform player)
    {
        yield return new WaitForSeconds(WaitAnimation1);
        attack.LetsAttack1(basicAttack, player);
    }

    IEnumerator Waitanimation2(Transform player)
    {
        yield return new WaitForSeconds(WaitAnimation2);
        attack.LetsAttack2(specialAttack, player);
    }

    IEnumerator Waitanimation3(Transform player)
    {
        yield return new WaitForSeconds(WaitAnimation3);
        attack.LetsAttack3(thirdAttack, player);
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(0.5f);
        float TeleportPosition = Random.Range(waypoints[0].position.x, waypoints[1].position.x);
        mob.transform.position = new Vector3(TeleportPosition, waypoints[0].position.y);
        
        if (health <= middlelife)
        {
            renderer.material.color = oldcolor;
            if (ColorUtility.TryParseHtmlString("#FF0000", out newcolor))
                renderer.material.color = newcolor;
        }
        isTeleporting = false;
    }

    private void ChasePlayer()
    {
        float DistMobPlayer = Mathf.Abs(playerToAttack.transform.position.x - mob.transform.position.x);
        if (DistMobPlayer < 17)
        {
            timer += Time.deltaTime;
            if (timer > WaitTime2)
            {
                timer = 0f;
                if (health > middlelife)
                {
                    animator.SetTrigger("Teleport");
                    isTeleporting = true;
                    StartCoroutine(WaitTeleportChase());
                }
                else
                {
                    int n = Random.Range(0, 100);
                    
                    if (n < 10)
                    {
                        isTeleporting = true;
                        
                        renderer.material.color = oldcolor;
                        if (ColorUtility.TryParseHtmlString("#E300FF", out newcolor))
                            renderer.material.color = newcolor;
                        animator.SetTrigger("Teleport");
                        StartCoroutine(WaitForTeleport());
                    }
                    else if (n >= 95)
                    {
                        isTeleporting = true;
                        
                        renderer.material.color = oldcolor;
                        if (ColorUtility.TryParseHtmlString("#FF9800", out newcolor))
                            renderer.material.color = newcolor;
                        health += 5;
                        
                        animator.SetTrigger("Teleport");
                        StartCoroutine(WaitTeleportVoid());
                    }
                    if (n < 95)
                    {
                        animator.SetBool("isMoving", true);
                        float posX = center.position.x;
                        float posXPlayer = playerToAttack.position.x;
                        if (posX < posXPlayer)
                            rb.velocity = new Vector2(speed, rb.velocity.y);
                        else if (posX > posXPlayer)
                            rb.velocity = new Vector2(-speed, rb.velocity.y);
                    }
                    isTeleporting = false;
                }
            }
        }
        else
            state = State.Waiting;
    }



    IEnumerator WaitTeleportVoid()
    {
        yield return new WaitForSeconds(0.5f);
        timer += Time.deltaTime;
        if (timer > WaitTime2)
        {
            timer = 0f;
            int random = Random.Range(0, 2);
            if (random == 1)
                mob.transform.position =
                    new Vector2(playerToAttack.position.x + 4, playerToAttack.position.y);
            else
                mob.transform.position =
                    new Vector2(playerToAttack.position.x - 4, playerToAttack.position.y);
        }
        isTeleporting = false;
    }

    IEnumerator WaitTeleportChase()
    {
        yield return new WaitForSeconds(0.5f);
        float DistMobPlayer = Mathf.Abs(playerToAttack.transform.position.x - mob.transform.position.x);
        float posX = center.position.x;
        float posXPlayer = playerToAttack.position.x;
        float posY = center.position.y;
        float posYPlayer = playerToAttack.position.y;
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
                        mob.transform.position = new Vector2(playerToAttack.position.x + 4f, posYPlayer);
                    else
                        mob.transform.position = new Vector2(playerToAttack.position.x + 4f, mob.transform.position.y);
                }
                else
                {
                    if (posY < posYPlayer)
                        mob.transform.position = new Vector2(playerToAttack.position.x - 4f, posYPlayer);
                    else
                        mob.transform.position = new Vector2(playerToAttack.position.x - 4f, mob.transform.position.y);
                }
            }
        }
        isTeleporting = false;
    }
}