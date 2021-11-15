//Si occupa del posizionamento dei veicoli nel podio
using UnityEngine;

public class PodioPlacement : MonoBehaviour
{
    //indica quanto in alto devono comparire i kart rispetto al podio
    [SerializeField]
    private float heightOffset = 5;
    //riferimenti ai kart dei corridori
    [SerializeField]
    private Transform playerKart = default, 
        jeepOrBoss = default, 
        bike = default, 
        flyingKart = default;

    //riferimenti ai podi
    [SerializeField]
    private Transform firstPlace = default,
        secondPlace = default,
        thirdPlace = default,
        fourthPlace = default;


    
    //private void OnEnable() { PlaceVehicles(); }

    /// <summary>
    /// Piazza i veicoli nel podio in base alla loro posizione di fine gara
    /// </summary>
    public void PlaceVehicles()
    {
        //ottiene l'array di posizioni nel podio dei veicoli sia nemici che del giocatore
        // 0 - jeep
        // 1 - macchina volante
        // 2 - moto
        // 3 - giocatore
        var positions = Checkpoints.GetVehiclesPositions();
        //cicla ogni posizione
        for (int pos = 0; pos < positions.Length; pos++)
        {
            //crea un riferimento locale al veicolo da posizionare
            Transform vehicleToMove = null;
            //l'indice indica il veicolo di cui si sta controllando la posizione
            switch (pos)
            {
                //JEEP OR BOSS
                case 0: { vehicleToMove = jeepOrBoss; break; }
                //MACCHINA VOLANTE
                case 1: { vehicleToMove = flyingKart; break; }
                //MOTO
                case 2: { vehicleToMove = bike; break; }
                //GIOCATORE
                case 3: { vehicleToMove = playerKart; break; }
                //IN CASO DI ERRORE, LO SCRIVE NELLA CONSOLE
                default: { Debug.LogError("USCITO DALL'ARRAY DI POSIZIONI"); break; }

            }
            //posiziona il veicolo ciclato nella posizione in classifica
            PositionVehicle(vehicleToMove, positions[pos] - 1);

        }
        //attiva il podio
        gameObject.SetActive(true);
        Debug.Log("Posizionati veicoli nel podio");
    }
    /// <summary>
    /// Posizionali il veicolo nel podio in base alla posizione in classifica
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="pos"></param>
    private void PositionVehicle(Transform vehicle, int pos)
    {
        //crea un riferimento locale al podio in cui posizionare il veicolo
        Transform podioPlace = null;
        //in base alla posizione ricevuta, il veicolo deve essere posizionato in un podio diverso
        switch (pos)
        {
            //PRIMO POSTO
            case 0: { podioPlace = firstPlace; break; }
            //SECONDO POSTO
            case 1: { podioPlace = secondPlace; break; }
            //TERZO POSTO
            case 2: { podioPlace = thirdPlace; break; }
            //QUARTO POSTO
            case 3: { podioPlace = fourthPlace; break; }
            //IN CASO DI ERRORE, LO SCRIVE NELLA CONSOLE
            default: { Debug.LogError("NON E' STATA TROVATA LA POSIZIONE DEL VEICOLO: " + vehicle + " -> " + pos); break; }

        }
        //il veicolo viene posizionato nel podio scelto
        vehicle.position = podioPlace.position + new Vector3(0, heightOffset, 0);
        //Debug.Log("Posizione " + vehicle.name + " = " + pos + " quindi è nel podio: " + podioPlace);
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
