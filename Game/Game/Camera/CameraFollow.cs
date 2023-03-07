using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;

    private Vector3 velocity;
    
    void Update()
    {
        transform.position = SmoothDamp(transform.position, player.transform.position,
            ref velocity, timeOffset);
    }
}
