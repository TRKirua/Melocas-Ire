using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    private float length, startpos, posY;
    public GameObject cam;
    public float parallaxEffect;

    // Start est appelé avant l'update de la première frame
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        posY = transform.position.y;
    }

    // Update est appelé une fois par frame
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect); // distance
        
        // nouvel emplacement du sprite
        transform.position = new Vector3(startpos - dist, posY, transform.position.z);
    }
}