using Pathfinding;
using UnityEngine;

namespace Boss_lv1
{
  public class Chase_target : MonoBehaviour
  {
      [Header("Pathfinding")] //chemin a suivre
      public Transform target; //cible à atteindre

      public float activateDistance = 12f; //distance à laquelle s'active le script je crois
      public float pathUpdateSeconds = 0.5f; //frequence à laquelle d'actualise l'algo

      [Header("Physics")] public float speed = 250f; //vitesse du perso

      public float nextWaypointDistance = 5f; //Distance à laquelle l'ennemi doit être pour qu'il se déplace vers le prochain point à la place de l'acutel
      
      
      public float jumpCheckOffset = 0.1f; //Collider


      [Header("Custom Behavior")] //Pour les ennemis stupides
      public bool followEnabled = true; // si = false l'ennemi suit pas la target
      
      public bool directionLookEnabled = true;

      private Path path; //chemin fourni par PathFinding
      private int currentWaypoint = 0; //Chemin actuel
      RaycastHit2D isGrounded; //pour le saut
      Seeker seeker; //script fournis par Seeker
      Rigidbody2D rb;

      public void Start()
      {
          seeker = GetComponent<Seeker>(); //trouve le seeker
          rb = GetComponent<Rigidbody2D>(); //trouve le rb

          InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds); //répète tout le temps le même script
      }


      private void FixedUpdate() //si la distance est la bonne et que follow=true on suit
      {
          if (TargetInDistance() && followEnabled)
          {
              PathFollow();
          }
      }

      private void UpdatePath()
      {
          if (followEnabled && TargetInDistance() && seeker.IsDone())
          {
              seeker.StartPath(rb.position, target.position, OnPathComplete);
              //si la distance est la bonne, que follow=true et que l'objet que l'on veut suivre a ete trouve on suit
          }
      }

      private void PathFollow()
      {
          if (path == null)
          {
              return;
          }
          
          if (currentWaypoint >= path.vectorPath.Count)
          {
              return; //si on a déjà parcouru tout les waypoints on sort de la boucle : on a atteint le joueur
          }

          // check si il entre en contact avec qq chose 
          Vector3 startOffset = transform.position -
                                new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);
          isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f); //retourne true si l'ennemi est sur le sol

          //calcule la direction de la force 
          Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
          Vector2 force = direction * speed * Time.deltaTime;
          

          // Movement
          rb.AddForce(force); //ajoute la force pour le mouvement

          // Calcule le prochain point ou aller 
          float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
          if (distance < nextWaypointDistance)
          {
              currentWaypoint++;
          }

          
          if (directionLookEnabled) //Fait pivoter le perso en fct de si il va vers la droite ou la gauche
          {
              if (rb.velocity.x > 0.05f)
              {
                  transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y,
                      transform.localScale.z);
              }
              else if (rb.velocity.x < -0.05f)
              {
                  transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y,
                      transform.localScale.z);
              }
          }
      }

      private bool TargetInDistance() //true si l'ennemi est assez proche pour pouvoir l'attaquer
      {
          return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
      }

      private void OnPathComplete(Path p)
      {
          if (!p.error)
          {
              path = p;
              currentWaypoint = 0;
          }
      }
  }
}
