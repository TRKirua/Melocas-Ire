using System.Collections;
using UnityEngine;

public class PassifOtema : MonoBehaviour
{
    private void FixedUpdate()
    {
        StartCoroutine(Passif());
    }


    IEnumerator Passif()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
