using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour
{
     PhotonView PV;
     public GameObject prefabselect;
     public GameObject prefabfix;

     void Awake()
     {
         PV = GetComponent<PhotonView>();

     }

     void Start()
     {
         if (PV.IsMine)
         {
             CreateController();
         }
     }

     void CreateController()
     {
         Debug.Log("test instantiate");
         Instantiate((prefabselect),new Vector3(0,0,0),Quaternion.identity );
         
     }
}
