using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class selectcharacter : MonoBehaviour
{
    
    // Start is called before the first frame update
     public void Choose()
     {
         gameObject.SetActive(false);
         PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Juh2"), new Vector3(-2, 2, 0),
             Quaternion.identity);
     }

     public void Choose2()
     {
         
         gameObject.SetActive(false);
         PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Moh2"), new Vector3(-2, 2, 0),
             Quaternion.identity);
     }

     public void Choose3()
     {
         gameObject.SetActive(false);
         PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Otema2"), new Vector3(-2, 2, 0),
             Quaternion.identity);
         
         
     }
     public void Choose4()
     {
     gameObject.SetActive(false);
              PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Kadare2"), new Vector3(-2, 2, 0),
                  Quaternion.identity);
     
     }
}
