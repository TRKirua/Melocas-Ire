using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyPatrol : MonoBehaviour
{
    private enum State
    {
        Waiting,
        ChaseTarget,
        Die,
        Hit,
    }
    
    
    public float speed;
    public Transform[] waypoints;
    private State state;

    public SpriteRenderer graphics;
    private Transform target;
    private int destPoint=0;
    
    // Start is called before the first frame update
    void Start()
    {
        target = waypoints[0];
    }
/* a modif
    private void FindTarget()
    {
        float targetRange = 30f;
        //cas en dessous : Player.Instance.GetPosition() = la position actuelle du joueur
        if (Vector3.Distance(transform.position, Player.Instance.GetPosition()) < targetRange)
        {
            
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime,Space.World);
        // Si l'ennemi est quasiment arrivé à sa destination
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];
            graphics.flipX = !graphics.flipX;
        }
    }
}