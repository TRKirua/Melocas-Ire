using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
public class selectfix : MonoBehaviour
{
    public GameObject prefabselectt;
    public void Choose()
    {
        gameObject.SetActive(false);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Juh2"), new Vector3(1.5f, 1.5f, 1.5f),
            Quaternion.identity);
    }
}
