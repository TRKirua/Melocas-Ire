using System;
using System.Collections;
using ScriptPlayer;
using UnityEngine;

public class Teleportal : MonoBehaviour
{
    private Animator animator;
    private LayerMask heros;
    
    public GameObject portal1;
    public GameObject portal2;
    private Vector2 pos;
    private float range;
    
    private void Start()
    {
        heros = LayerMask.GetMask("Player");
        pos = new Vector2(portal1.transform.position.x, portal1.transform.position.y - 2);
        range = 0.8f;
    }

    private void Update()
    {
        Collider2D[] heroesPortal = Physics2D.OverlapCircleAll(pos, range, heros);

        for (int i = 0; i < heroesPortal.Length; i++)
        {
            if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0)
            {
                heroesPortal[i].GetComponent<Move>().portail = false;
                heroesPortal[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                heroesPortal[i].GetComponent<Animator>().SetTrigger("portail");
                StartCoroutine(Teleport(heroesPortal[i]));
            }
        }
    }

    IEnumerator Teleport(Collider2D player)
    {
        yield return new WaitForSeconds(0.4f);
        player.transform.position = new Vector2(portal2.transform.position.x, portal2.transform.position.y);
        player.GetComponent<Move>().portail = true;
    }
}
