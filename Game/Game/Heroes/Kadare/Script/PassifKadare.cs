using System.Collections;
using System.Collections.Generic;
using Niveau_1.Boss_lv1.Scripts;
using ScriptPlayer;
using UnityEngine;

public class PassifKadare : MonoBehaviour
{
    public Animator anim;
    
    private int FirstDamage = 3; //première partie de l'attaque ultime
    private int SecondDamage = 7; //deuxième partie
    private float recharge = 30; //temps de rechargement
    
    public Transform passifPoint1; //premiere zone de dommages
    public Transform passifPoint2; //deuxieme zone
    public Vector2 passifRadius1; //rayon de la 1ere zone
    public Vector2 passifRadius2; //rayon de la 2eme zone
    public LayerMask ennemies; //Layer des ennemis
    
    public bool isPassif; //est en train de faire le passif
    public bool canPassif; //peut activer le passif


    void FixedUpdate()
    {
        if (canPassif && !isPassif)
        {
            if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0) //bouton bas
            {
                if (!GetComponent<Attack1Kadare>().isAttacking1Kadare && !GetComponent<Attack2Kadare>().isAttacking2Kadare) //s'il n'attaque pas deja
                {
                    isPassif = true; //est en train de faire le passif
                    GetComponent<Move>().isAttacking2 = true; //utlisation de la mm variable que l'attaque speciale pour ne pas bouger

                    GetComponent<Health>().passifActivated = true; //pour ne pas se prendre de degats
                    canPassif = false;
                    
                    //a cause des tailles de frames differentes, on doit replacer le joueur lors du passif
                    if (transform.localScale.x > 0) //si tourner a droite 
                        transform.position = new Vector3(transform.position.x + 3.14f, transform.position.y + 1.9f); //replacement du joueur pr l'activation du passif
                    else //si tourner a gauche
                        transform.position = new Vector3(transform.position.x - 3.14f, transform.position.y + 1.9f); //replacement du joueur pr l'activation du passif
                    
                    anim.SetTrigger("passif"); //animation du passif
                    StartCoroutine(LetsAttackPassif());
                    recharge = 30; //remise à 30s
                }
            }
        }

        else
        {
            if (recharge < 0) //si temps ecoulé
            {
                recharge = 30; //remise à 30s
                canPassif = true; //peut utiliser le passif
            }
            else
                recharge -= 0.02f; //diminution du temps de rechargement
        }
    }
    

    IEnumerator LetsAttackPassif()
    {
        yield return new WaitForSeconds(0.4f); //attente de l'animation de la première vague de dégâts
        
        Collider2D[] ennemiesToDamage = Physics2D.OverlapBoxAll(passifPoint1.position, passifRadius1, 90, ennemies);
        //Ceux qui sont dans le champ d'attaque

        //première vague de dégâts
        for (int i = 0; i < ennemiesToDamage.Length; i++)
            if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
                ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(FirstDamage);
        
        yield return new WaitForSeconds(0.2f); //attente de l'animation de la deuxième vague de dégâts
        
        Collider2D[] ennemiesToDamage2 = Physics2D.OverlapBoxAll(passifPoint2.position, passifRadius2, 90, ennemies);
        //Ceux qui sont dans le champ d'attaque

        //deuxième vague de dégâts
        for (int i = 0; i < ennemiesToDamage.Length; i++)
            if (ennemiesToDamage[i].GetComponent<EnnemyS>().state != EnnemyS.State.Die)
                ennemiesToDamage[i].GetComponent<EnnemyS>().TakeDamage(SecondDamage);
        
        yield return new WaitForSeconds(0.575f); //attente de la fin de l'animation
        
        isPassif = false; //n'est plus en train d'attaquer
        GetComponent<Move>().isAttacking2 = false; //pour perettre de rebouger
        GetComponent<Health>().passifActivated = false; //pour permettre de se faire toucher
        
        if (transform.localScale.x > 0) //s'il est tourner à droite
            transform.position = new Vector3(transform.position.x - 3.14f, transform.position.y - 1.9f); //replacement du joueur
        else //sinon
            transform.position = new Vector3(transform.position.x + 3.14f, transform.position.y - 1.9f); //replacement du joueur
    }



    private void OnDrawGizmos() //permet de regler les zones sur unity
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(passifPoint1.position, passifRadius1);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(passifPoint2.position, passifRadius2);
    }
}
