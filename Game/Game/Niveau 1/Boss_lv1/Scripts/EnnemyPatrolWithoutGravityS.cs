using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemyPatrolWithoutGravityS : EnnemyS
{
    
    public GameObject Attack_projectile; //Projectile
    public GameObject Special_projectile;
    public Transform appearPoint; //Point d'apparition du projectile
    private float bAttackRate = 0.4f; //Fréquence d'attaque basique
    private float bNextAttack; //Prochaine attaque basique
    public SpriteRenderer boss;
    
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Waiting;
        target = waypoints[0];
        length = waypoints.Length;
        PlayerRange = 16f;
    }



    // Update is called once per frame
    void Update()
    {
        if (isFreeze)
        {
            animator.Rebind();
            animator.Update(0f);
        }
        
        if (transform.localScale.x >0)
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
        
        // Si il n'a plus de vie
        if (state == State.Die)
        {
            rb.velocity = Vector2.zero;
            cd.enabled = false;
            animator.SetTrigger("isDead");
            
            float ground = 63f;
            
            Vector2 Sol = new Vector2(mob.position.x, ground);
            mob.position = Vector2.MoveTowards(mob.position, Sol, 30);
        }


        else
        {
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
                animator.SetBool("isChasing", false);
                
                float distToWaypoint =
                    Vector3.Distance(mob.position, target.position); //Distance au prochain point

                Vector3 dir = target.position - mob.position;
                mob.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

                if (distToWaypoint < 0.3f)
                {
                    Vector3 f = new Vector3(-transform.localScale.x, 0.6f, 1);
                    destPoint = (destPoint + 1) % length; //Prochain waypoint
                    target = waypoints[destPoint]; //Cible
                    transform.localScale = f; //Flip le joueur
                }

                if (distToPlayer < PlayerRange) //Si le joueur est assez proche il commence à le suivre
                    state = State.ChaseTarget;
            }
            
            
            
            // Chasse le player
            else if (state == State.ChaseTarget)
            {
                if (!isFreeze)
                {
                    if (playerToAttack.position.x > mob.position.x)
                        transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                    else
                        transform.localScale = new Vector3(0.6f, 0.6f, 1);


                    if (distToPlayer > 30f) //Distance à partir de laquelle il arrête de suivre le joueur
                        state = State.Waiting;

                    else
                    {
                        animator.SetBool("isChasing", true); //animation

                        if (distToPlayer > 14f)
                            ChasePlayer(playerToAttack); //Chasse le player
                        else
                        {
                            if (mob.position.y > playerToAttack.position.y - 0.5f ||
                                mob.position.y < playerToAttack.position.y - 1.5f)
                                RightSpot(playerToAttack); //Se place à la bonne hauteur
                            else if (Time.time >= bNextAttack && !isAttackingEnnemyS)

                            {
                                bNextAttack = Time.time + 1f / bAttackRate;
                                isAttackingEnnemyS = true;
                                AttackPlayer(basicAttack, specialAttack);
                            }
                        }
                    }
                }
            }
        }
    }
    
    
    
    //Attaque le joueur
    private void AttackPlayer(int basicattack, int specialattack)
    {
        int n = Random.Range(0, 3);
        

        if (n == 0) // Attaque spéciale 1 chance sur 3
        {
            animator.SetTrigger("attackspecial");
            StartCoroutine(WaitForAnimation2());
        }
        

        else // Attaque basique 2 chance sur trois
        {
            animator.SetTrigger("attackbasique");
            StartCoroutine(WaitForAnimation());
        }
        
        
    }
    
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.75f);
        Instantiate(Attack_projectile, appearPoint.position, appearPoint.rotation);
        isAttackingEnnemyS = false;
    }
    
    IEnumerator WaitForAnimation2()
    {
        yield return new WaitForSeconds(0.6f);
        Vector3 p = new Vector3(appearPoint.position.x, appearPoint.position.y - 1);
        Instantiate(Special_projectile, p, appearPoint.rotation);
        isAttackingEnnemyS = false;
    }



    // Chasser le joueur
    private void ChasePlayer(Transform player)
    {
        float posY = mob.position.y;
        float posYPlayer = player.position.y;
        float posX = mob.position.x;
        float posXPlayer = player.position.x;

        if (posY < posYPlayer)
        {
            if (posX < posXPlayer)
                rb.velocity = new Vector2(speed, speed);

            else if (posX > posXPlayer)
                rb.velocity = new Vector2(-speed, speed);
        }

        else if (posY >= posYPlayer)
        {
            if (posX < posXPlayer)
                rb.velocity = new Vector2(speed, -speed);

            else if (posX > posXPlayer)
                rb.velocity = new Vector2(-speed, -speed);
        }

        else
        {
            if (posX < posXPlayer)
                rb.velocity = new Vector2(speed, 0);

            else if (posX > posXPlayer)
                rb.velocity = new Vector2(-speed, 0);
        }
    }


    
    //Se placer à la bonne hauteur d'attaque
    private void RightSpot(Transform player)
    {
        float posY = mob.position.y;
        float posYPlayer = player.position.y-1;
        
        if (posY > posYPlayer)
            rb.velocity = new Vector2(0, -speed);
        if (posY < posYPlayer)
            rb.velocity = new Vector2(0, speed);
    }
}
