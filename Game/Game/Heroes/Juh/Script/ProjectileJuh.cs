using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;


public class ProjectileJuh : MonoBehaviour
{
    public Rigidbody2D rb; //Rigidbody projectile
    private float speed = 25f; //Vitesse projectile
    private int lifetime = 1; //Temps de vie
    private float n;

    public int MagicAttack = 5; // Puissance attaque magique

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void Update()
    {
        n += 0.01f;
        
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
