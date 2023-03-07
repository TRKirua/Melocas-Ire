using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassifMoh2 : MonoBehaviour
{
    public bool destroy;
    
    private void Update()
    {
        if (destroy)
            Destroy(gameObject);
    }
}
