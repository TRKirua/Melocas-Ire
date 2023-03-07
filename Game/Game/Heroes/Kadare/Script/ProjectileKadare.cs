
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class ProjectileKadare : MonoBehaviour
{
    private float lifetime = 4.3f; //Temps de vie
    private float n;

    public int MagicAttack = 7; // Puissance attaque magique

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
