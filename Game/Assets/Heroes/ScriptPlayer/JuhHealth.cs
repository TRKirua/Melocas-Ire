using UnityEngine;

public class JuhHealth : MonoBehaviour
{
    public int maxHealth = 13; // Vie maxiamle
    public int currentHealth; // Vie actuelle

    public HealthBar healthBar; // Barre de vie visuelle
    
    void Start()
    {
        currentHealth = maxHealth; // Assigner les points de vie de base
        healthBar.SetMaxHealth(maxHealth); // Mettre à jour le visuel
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; // Retirer points de vie
        healthBar.SetHealth(currentHealth); // Mettre à jour le visuel
    }
}
