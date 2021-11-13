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
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            //spawna la granata nel punto di spawn dedsignato con la rotazione del gameobject a cui è assegnato lo script
            Instantiate(grenade, bombpoint.position, transform.rotation);

        }

    }
}
