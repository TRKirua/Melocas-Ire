using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;
public class AttackDarkJuhS : MonoBehaviour
{
    public Transform basicAttackPos, specialAttackPos, thirdAttackPos; 
    public float basicAttackRange, specialAttackRange, thirdAttackRange; //Rayon d'attaque basique
    private float currenthealth;
    public LayerMask heros; //Default
    public bool isHit;
    private void Start()
    {
        currenthealth = GetComponent<EnnemyS>().health;
    }
    public bool GetHit()
    { 
        isHit = false;
        
        if (GetComponent<EnnemyS>().health != currenthealth)
        {
            currenthealth = GetComponent<EnnemyS>().health;
            isHit = true;
        }
        return isHit;
    }
    public void LetsAttack1(int damage, Transform player)
    {
        if (GetComponent<MoveDarkJuhS>().alreadyattacked)
        {
            Collider2D[] heroToDamage =
                Physics2D.OverlapCircleAll(basicAttackPos.position, basicAttackRange, heros);
            for (int j = 0; j < heroToDamage.Length; j++)
            {
                if (heroToDamage[j].GetComponent<DeathPacito>().canhit)
                    heroToDamage[j].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MoveDarkJuhS>().NextAttack = 0;
            StartCoroutine(WaitForTeleport(player));
        }
        GetComponent<MoveDarkJuhS>().isAttacking = false;
    }
    
    public void LetsAttack2(int damage, Transform player)
    {
        if (GetComponent<MoveDarkJuhS>().alreadyattacked)
        {
            Collider2D[] heroToDamage2 =
                Physics2D.OverlapCircleAll(specialAttackPos.position, specialAttackRange, heros);
            for (int i = 0; i < heroToDamage2.Length; i++)
            {
                if (heroToDamage2[i].GetComponent<DeathPacito>().canhit)
                    heroToDamage2[i].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MoveDarkJuhS>().NextAttack = 0;
            StartCoroutine(WaitForTeleport(player));
        }
        GetComponent<MoveDarkJuhS>().isAttacking = false;
    }
    
    public void LetsAttack3(int damage, Transform player)
    {
        if (GetComponent<MoveDarkJuhS>().alreadyattacked)
        {
            Collider2D[] heroToDamage =
                Physics2D.OverlapCircleAll(thirdAttackPos.position, thirdAttackRange, heros);
            for (int i = 0; i < heroToDamage.Length; i++)
            {
                if (heroToDamage[i].GetComponent<DeathPacito>().canhit)
                    heroToDamage[i].GetComponent<Health>().TakeDamage(damage);
            }
        }
        else
        {
            GetComponent<MoveDarkJuhS>().NextAttack = 0;
            StartCoroutine(WaitForTeleport(player));
        }
        GetComponent<MoveDarkJuhS>().isAttacking = false;
    }
    
    IEnumerator WaitForTeleport(Transform player)
    {
        yield return new WaitForSeconds(0.5f);
        float TeleportPosition = Random.Range(GetComponent<MoveDarkJuhS>().waypoints[0].position.x,
            GetComponent<MoveDarkJuhS>().waypoints[1].position.x);
        float DistMobPlayer = Mathf.Abs(player.position.x - TeleportPosition);
        if (DistMobPlayer < 10)
        {
            TeleportPosition = Random.Range(player.position.x - (10 - DistMobPlayer),
                player.position.x + (10 - DistMobPlayer));
            GetComponent<MoveDarkJuhS>().mob.transform.position = new Vector3(TeleportPosition,
                GetComponent<MoveDarkJuhS>().waypoints[0].position.y);
        }
        else
            GetComponent<MoveDarkJuhS>().mob.transform.position = new Vector3(TeleportPosition,
                GetComponent<MoveDarkJuhS>().waypoints[0].position.y);
        
        GetComponent<MoveDarkJuhS>().alreadyattacked = true;
        GetComponent<MoveDarkJuhS>().isTeleporting = false;
    }
    
    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicAttackPos.position, basicAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(specialAttackPos.position, specialAttackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(thirdAttackPos.position, thirdAttackRange);
    }
}