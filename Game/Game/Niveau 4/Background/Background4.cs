using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background4 : MonoBehaviour
{
	private float length, startpos; 
	public GameObject cam; 
	public float parallaxEffect;


	// Start est appelé avant l'update de la première frame
    void Start()
    {
	    startpos = transform.position.x; //position où il commence
	    length = GetComponent<SpriteRenderer>().bounds.size.x; //taille du sprite en x
    }

    // Update est appelé une fois par frame
    void FixedUpdate()
    {
	    float temp = (cam.transform.position.x * (1 - parallaxEffect)); //le temps 
	    float dist = (cam.transform.position.x * parallaxEffect); // la distance

	    // nouvel emplacement du sprite
	    transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z); 

	    //permet de répéter l'image
	    if (temp > startpos + length) 
		    startpos += length;
	    else if (temp < startpos - length)
		    startpos -= length;
    }
}
