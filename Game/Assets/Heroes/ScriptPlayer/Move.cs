using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptPlayer
{
    public class Move : MonoBehaviour
    {
        public float Speed; // Vitesse du joueur (entrée dans unity)
        public float JumpForce; // Force de saut du joueur (entrée dans unity)
        
        private bool isJumping; // Savoir si le joueur est en train de sauter (true ou false)
        private bool isOnGround; // Savoir si le player est au sol (true ou false)
        
        public Transform groundCheck; // Vérification du contact avec le sol
        public float groundCheckRadius; // Rayon du cercle de vérification
        public LayerMask collisionLayers;

        public Rigidbody2D rb; // Gravité du player
        public Animator animator; // Animation
        public SpriteRenderer spriterend; // Image du joueur

        private Vector3 velocity = Vector3.zero; // Vélocité
        private float horizontalMovement; // Direction mouvement

        private bool flip1; // Mettre sur la bonne position
        private bool flip2 = true; // Idem
        public Transform player;


        void Update()
        {
            if (Input.GetButtonDown("Jump") && isOnGround) // Si on appuie sur sauter et si le player est au sol
                isJumping = true; // isJumping devient vrai

            Flip(rb.velocity.x, ref player); // Lance Flip avec x
            
            if (flip1 == flip2) // Vérifie si on doit repositionner le joueur donc s'il vient de se retourner
            {
                if (player.localScale.x > 0)
                    transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);

                flip1 = !flip1;
            }
            
            float playerVelocity = Mathf.Abs(rb.velocity.x); // Valeur absolue du mouvement
            animator.SetFloat("Speed", playerVelocity); // Animation

            if (rb.velocity.y >= 10)
                animator.SetBool("isJumping", true);
            
            if (rb.velocity.y <= 0)
                animator.SetBool("isJumping", false);
        }
        
        
        
        void FixedUpdate()
        {
            isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers); // Vérifiez si le joueur est au sol
            
            horizontalMovement = Input.GetAxis("Horizontal") * Speed * Time.deltaTime; // Calculer le mouvement et définir la direction
            
            MovePlayer(horizontalMovement); // Lance MovePlayer avec le mouvement calculé
        }

        
        
        void MovePlayer(float _horizontalMovement)
        {
            Vector3 target = new Vector2(_horizontalMovement, rb.velocity.y); // Calculer la vélocité du player
            rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref velocity, .05f); // Effectuer le mouvement

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, JumpForce)); // Saut
                isJumping = false; // isJumping est remis à faux
            }
        }


        void Flip(float _velocity, ref Transform scale) // Retourne le player selon son mouvement (droite ou gauche) (sa boîte de collision aussi)
        {
            // (Notre joueur à nous est déjà retourné de base car on l'a dessiné dans l'autre sens)
            if (_velocity > 0.1f && scale.localScale.x < 0)
            {
                scale.localScale = new Vector3(0.25f, 0.25f, 1);
                flip2 = !flip2;
            }
            
            else if (_velocity < -0.1f && scale.localScale.x > 0)
            {
                scale.localScale = new Vector3(-0.25f, 0.25f, 1);
                flip2 = !flip2;
            }
        }


        private void OnDrawGizmos() // Fonction qui permet juste de voir le cercle qui nous permettra de régler le contact du player avec le sol
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
