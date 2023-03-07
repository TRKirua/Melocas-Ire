using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Niveau_1.Boss_lv1.Scripts;
using UnityEngine;

public class RandomSizeGroblinS : MonoBehaviour
{
    private float hp;
    private Vector3 scale, mobtranformed;
    public EnnemyS mob;
    // Start is called before the first frame update
    void Start()
    {
        hp = mob.health;
        scale = new Vector3(mob.transform.localScale.x * 2,mob.transform.localScale.y * 2);
    }

    // Update is called once per frame
    void Update()
    {
        mobtranformed = mob.transform.localScale;
        if (hp != mob.health)
        {
            if (mob.GetComponent<EnnemyS>().state != EnnemyS.State.Die)
            {
                mobtranformed *= Random.Range(0.1f, 2f);

                while (mobtranformed.x > scale.x)
                    mobtranformed *= Random.Range(0.1f, 2f);

                mob.transform.localScale = mobtranformed;
                hp = mob.health;
            }
        }
    }
}
