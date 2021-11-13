using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwGranade : MonoBehaviour
{
    [SerializeField]
    private GameObject grenade;
    [SerializeField]
    private Transform bombpoint;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(spawnBomb());
    }

    IEnumerator spawnBomb()
    {
        yield return new WaitForSeconds(10);
        //spawna la granata nel punto di spawn dedsignato con la rotazione del gameobject a cui è assegnato lo script
        Instantiate(grenade, bombpoint.position, transform.rotation);
        yield return new WaitForSeconds(5);
        StartCoroutine(spawnBomb());



    }
}
