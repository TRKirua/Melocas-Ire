using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassBackGroundLevel5: MonoBehaviour
{
    public GameObject objet;
    public Renderer RendOfObjet;
    private int LayerDeBase;
    public int NewLayer;

    private void Start()
    {
        LayerDeBase = objet.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (objet.transform.position.x > 86.83f && objet.transform.position.x < 93.31f)
            RendOfObjet.sortingOrder = NewLayer;
        else
            RendOfObjet.sortingOrder = LayerDeBase;
    }
}