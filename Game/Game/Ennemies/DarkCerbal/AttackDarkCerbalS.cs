using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;
public class AttackDarkCerbalS : MonoBehaviour
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
    public void LetsAttack1(int damage)
    {
        if (GetComponent<MobDarkCerbalS>().alreadyattacked && !GetComponent<MobDarkCerbalS>().isDead)
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
            GetComponent<MobDarkCerbalS>().NextAttack = 0;
        }

        GetComponent<MobDarkCerbalS>().isAttacking = false;
    }

    public void LetsAttack2(int damage)
    {
        if (GetComponent<MobDarkCerbalS>().alreadyattacked && !GetComponent<MobDarkCerbalS>().isDead)
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
            GetComponent<MobDarkCerbalS>().NextAttack = 0;
        }
        GetComponent<MobDarkCerbalS>().isAttacking = false;
    }

    public IEnumerator WaitForTeleport(Transform player)
    {
        yield return new WaitForSeconds(1f);
        float TeleportPosition = Random.Range(GetComponent<MobDarkCerbalS>().waypoints[0].position.x,
            GetComponent<MobDarkCerbalS>().waypoints[1].position.x);
        float DistMobPlayer = Mathf.Abs(player.position.x - TeleportPosition);
        if (DistMobPlayer < 10)
        {
            TeleportPosition = Random.Range(player.position.x - (10 - DistMobPlayer), player.position.x + (10 - DistMobPlayer));

            GetComponent<MobDarkCerbalS>().mob.transform.position = new Vector3(TeleportPosition,
                GetComponent<MobDarkCerbalS>().waypoints[0].position.y);
        }
        else
            GetComponent<MobDarkCerbalS>().mob.transform.position = new Vector3(TeleportPosition,
                GetComponent<MobDarkCerbalS>().waypoints[0].position.y);

        GetComponent<MobDarkCerbalS>().alreadyattacked = true;
        GetComponent<MobDarkCerbalS>().isTeleporting = false;
    }

    private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(basicAttackPos.position, basicAttackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(specialAttackPos.position, specialAttackRange);
    }
}