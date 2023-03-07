using System.Collections;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public string HexaColor;
    private Color newcolor, othercolor;
    public GameObject cam;
    public Renderer renderer;
    private bool change, alreadychanged, alreadydivided;
    private float time_;
    public float time,t ; 

    // Start is called before the first frame update
    void Start()
    {
        newcolor = renderer.material.color;
        othercolor = renderer.material.color;
        t = 0;
        change = false;
        alreadychanged = true;
        alreadydivided = true;
        time = 10;
        time_ = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.transform.position.x < 282)
            alreadydivided = true;
        
        if (alreadydivided)
        {
            if (cam.transform.position.x >= 282)
            {
                time /= 2;
                alreadydivided = false;
            }
            else
                time = time_;
        }

        StartCoroutine(ChangeBlack());
    }

    IEnumerator ChangeBlack()
    {
        if (!change)
        {
            yield return new WaitForSecondsRealtime(time);

            if (alreadychanged)
            {
                if (ColorUtility.TryParseHtmlString("#" + HexaColor, out newcolor))
                {
                    renderer.material.color = Color.Lerp(othercolor, newcolor, Mathf.Lerp(0,1,t));

                    if (t < 1)
                        t += 0.01f;

                    if (renderer.material.color == newcolor)
                    {
                        t = 0;
                        alreadychanged = false;
                    }
                }
            }

            change = true;
        }
             
        else
        {
            yield return new WaitForSecondsRealtime(time);

            if (!alreadychanged)
            {
                renderer.material.color = Color.Lerp(newcolor, othercolor, Mathf.Lerp(0,1,t));

                if (t < 1)
                    t += 0.01f;

                if (renderer.material.color == othercolor)
                {
                    t = 0;
                    alreadychanged = true;
                }
            }

            change = false;
        }
    }
}
