
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;


public class ProjectileMoh : MonoBehaviour
{
    private float lifetime = 3.8f; //Temps de vie
    private float n;

    public int MagicAttack = 6; // Puissance attaque magique

    private void FixedUpdate()
    {
        n += 0.1f;

        if (n > lifetime)
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D hitbox)
    {
        EnnemyS EnnemyS = hitbox.GetComponent<EnnemyS>();
        
        if (EnnemyS != null)
        {
            EnnemyS.TakeDamage(MagicAttack);
        }
    }
}