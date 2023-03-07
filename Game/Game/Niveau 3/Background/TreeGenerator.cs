using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
	private float startpos, startposY;
	private float parallaxEffect;
	private Vector3 init;
	private Camera _cam;

	// Start is called before the first frame update
	void Start()
    {
	    _cam = Camera.main;
	    startpos = transform.position.x; //position où il commence
	    parallaxEffect = Random.Range(2f, 4f);
	    init = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
	    float height = 2f * _cam.orthographicSize;
	    float width = height * _cam.aspect;
	    
	    float temp = (_cam.transform.position.x * (1 - parallaxEffect)); //le temps 
	    float dist = (_cam.transform.position.x * parallaxEffect); // la distance

	    // nouvel emplacement du sprite
	    transform.position = new Vector3(startpos + dist, startposY - height/4, transform.position.z);
	    
	    //permet de répéter l'image
	    if (temp > startpos + width)
	    {
		    startpos += Random.Range(width + width/2 + width/4,width*2);
		    transform.localScale = init;
		    
		    float scale =  Random.Range(0.5f,3f);
		    transform.localScale *= scale;

		    startposY *= scale;
	    }
	    else if (temp < startpos - width)
	    {
		    startpos -= Random.Range(width + width/2 + width/4,width*2);
		    transform.localScale = init;
		    
		    float scale =  Random.Range(0.5f,3f);
		    transform.localScale *= scale;
		    
		    startposY *= scale;

	    }
    }
}
