using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
	private float a;
	public Color colorchanging;
	public new Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
	    renderer = gameObject.GetComponent<Renderer>();
	    colorchanging = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
	    if (transform.position.x <= -350f)
	    {
		    for (float f = 1f; f >= -0.05f; f -= 0.01f)
		    {
			    Color truc = renderer.material.color;
			    truc.a = f;
			    renderer.material.color = truc;
		    }
	    }
	    else
	    {
		    for (float f = 0f; f <= 1f; f += 0.01f)
		    {
			    Color truc = renderer.material.color;
			    truc.a = f;
			    renderer.material.color = truc;
		    }
	    }
    }
}
