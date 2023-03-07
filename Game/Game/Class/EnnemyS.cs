using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

namespace Niveau_1.Boss_lv1.Scripts
{
    public class EnnemyS : MonoBehaviour

    {
        public enum State
        {
            Waiting,
            ChaseTarget,
            Die,
        }

        public Transform mob;
        public State state; //Etat

        public float speed; //Vitesse
        public int health; //PV

        public int basicAttack; //Dégâts attaque basique
        public int specialAttack; //Dégâts attaque spéciale
        public bool isAttackingEnnemyS;

        public Rigidbody2D rb; //Rigidbody
        public Animator animator; //Animation
        public Collider2D cd; //Collider

        public Transform[] waypoints; //Liste de waypoints
        protected int length; //Nombre de waypoints;

        public List<GameObject> listOfPlayers;

        protected float PlayerRange; //Distance à partir de laquelle il commence à attaquer le joueur

        protected Transform target; //Cible
        protected int destPoint; //Cible changeante
        public SpriteRenderer graphics; //Aperçu mob
        
        public Animator camAnim;//Shake caméra
        public bool isFreeze; //est gelé ou non (passif juh)


        public void TakeDamage(int damage)
        {
            if (state == State.ChaseTarget)
            {
                if (!isAttackingEnnemyS)
                    animator.SetTrigger("isHit"); //animation des dégâts pris

                health -= damage; //baisse de pv
                camAnim.SetTrigger("shake");

                if (health < 1)
                    state = State.Die;
                else
                    state = State.ChaseTarget;
            }
        }
    }
}