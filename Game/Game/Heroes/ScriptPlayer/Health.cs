using System.Collections;
using JetBrains.Annotations;
using ScriptPlayer;
using UnityEngine;
using UnityEngine.EventSystems;

public class Health : MonoBehaviour
{
    public int maxHealth; // Vie maxiamle
    public int currentHealth; // Vie actuelle

    public HealthBar healthBar; // Barre de vie visuelle
    public Animator animator;
    public bool passifActivated;

    void Start()
    {
        currentHealth = maxHealth; // Assigner les points de vie de base
        healthBar.SetMaxHealth(maxHealth); // Mettre à jour le visuel
    }

    public void TakeDamage(int damage)
    {
        if (!passifActivated)
        {
            animator.SetTrigger("isHit"); // Animation dégâts

            if (GetComponent("PassifMoh") as PassifMoh != null)
                if (GetComponent<PassifMoh>().hasShield)
                    damage -= 1;

            currentHealth -= damage; // Retirer points de vie
            healthBar.SetHealth(currentHealth); // Mettre à jour le visuel
            transform.GetComponent<Move>().isNotHit = false;
            
            if (currentHealth < 1)
                animator.SetTrigger("isDead");
        }
    }
}
