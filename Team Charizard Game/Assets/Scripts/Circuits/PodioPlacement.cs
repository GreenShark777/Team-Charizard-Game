//Si occupa del posizionamento dei veicoli nel podio
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodioPlacement : MonoBehaviour
{

    private float heightOffset = 5;

    [SerializeField]
    private Transform playerKart = default;


    public void PlaceVehicles()
    {


        Debug.Log("Posizionati veicoli nel podio");
    }

}
