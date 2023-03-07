using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class animenu : MonoBehaviour
{
   
    public GameObject play;
    

    public bool IsOver = false;

    // Start is called before the first frame update
    public void PointerEnter()
    {
        IsOver = true;
        LeanTween.scale(play.GetComponent<RectTransform>(), new Vector3(1.2f, 1.2f, 1.2f), 0.1f);
       
    }

    public void PointerExit()
    {
        IsOver = false;
        LeanTween.scale(play.GetComponent<RectTransform>(), new Vector3(1, 1, 1), 0.1f);
        
    }

    
    
}
