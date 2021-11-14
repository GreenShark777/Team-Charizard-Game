//Si occupa del posizionamento dei veicoli nel podio
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodioPlacement : MonoBehaviour
{
    //indica quanto in alto devono comparire i kart rispetto al podio
    [SerializeField]
    private float heightOffset = 5;

    //riferimenti ai kart dei corridori
    [SerializeField]
    private Transform playerKart = default, 
        jeep = default, 
        bike = default, 
        flyingKart = default;

    //riferimenti ai podi
    [SerializeField]
    private Transform firstPlace = default,
        secondPlace = default,
        thirdPlace = default,
        fourthPlace = default;


    
    private void Awake()
    {

        PlaceVehicles();

    }

    public void PlaceVehicles()
    {
        //ottiene l'array di posizioni nel podio dei veicoli sia nemici che del giocatore
        // 0 - jeep
        // 1 - macchina volante
        // 2 - moto
        // 3 - giocatore
        var positions = Checkpoints.GetVehiclesPositions();

        for (int pos = 0; pos < positions.Length; pos++)
        {

            Transform vehicleToMove = null;

            switch (pos)
            {

                case 0: { vehicleToMove = jeep; break; }
                case 1: { vehicleToMove = flyingKart; break; }
                case 2: { vehicleToMove = bike; break; }
                case 3: { vehicleToMove = playerKart; break; }
                default: { Debug.LogError("USCITO DALL'ARRAY DI POSIZIONI"); break; }

            }

            PositionVehicle(vehicleToMove, positions[pos] - 1);

        }

        Debug.Log("Posizionati veicoli nel podio");
    }

    private void PositionVehicle(Transform vehicle, int pos)
    {

        Transform podioPlace = null;

        switch (pos)
        {

            case 0: { podioPlace = firstPlace; break; }
            case 1: { podioPlace = secondPlace; break; }
            case 2: { podioPlace = thirdPlace; break; }
            case 3: { podioPlace = fourthPlace; break; }
            default: { Debug.LogError("NON E' STATA TROVATA LA POSIZIONE DEL VEICOLO: " + jeep + " -> " + pos); break; }

        }

        vehicle.position = podioPlace.position + new Vector3(0, heightOffset, 0);
        Debug.Log("Posizione " + vehicle.name + " = " + pos + " quindi è nel podio: " + podioPlace);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(firstPlace.position + new Vector3(0, heightOffset, 0), 1);
        Gizmos.DrawWireSphere(secondPlace.position + new Vector3(0, heightOffset, 0), 1);
        Gizmos.DrawWireSphere(thirdPlace.position + new Vector3(0, heightOffset, 0), 1);
        Gizmos.DrawWireSphere(fourthPlace.position + new Vector3(0, heightOffset, 0), 1);

    }

}
