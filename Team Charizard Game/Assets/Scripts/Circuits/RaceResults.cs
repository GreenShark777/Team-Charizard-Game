//Si occupa di mostrare i risultati della gara nella UI
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceResults : MonoBehaviour
{
    //riferimento al bottone di riprova
    [SerializeField]
    private GameObject retryButton = default;
    //riferimento allo script che si occupa di posizionare i veicoli nel podio
    [SerializeField]
    private PodioPlacement pp = default;

    [SerializeField]
    private Text endResultText = default, //riferimento al testo che indica al giocatore se ha vinto o meno
        firstFinishedTimeText = default, //riferimento al testo che indica il tempo in cui ha finito il kart al primo posto
        secondFinishedTimeText = default, //riferimento al testo che indica il tempo in cui ha finito il kart al secondo posto
        thirdFinishedTimeText = default, //riferimento al testo che indica il tempo in cui ha finito il kart al terzo posto
        fourthFinishedTimeText = default; //riferimento al testo che indica il tempo in cui ha finito il kart al quarto posto

    [SerializeField]
    private string jeepFinishedTime = "JEEP", //indica il tempo in cui la jeep finisce il circuito
        flyingCarFinishedTime = "FLYING", //indica il tempo in cui la macchina volante finisce il circuito
        bikeFinishedTime = "BIKE"; //indica il tempo in cui la moto finisce il circuito

    //indica il tempo in cui il giocatore finisce il circuito
    private string playerFinishedTime;
    //indica quanto tempo deve durare la transizione
    [SerializeField]
    private float transitionTime = 1;


    public void UpdateEndResults(bool playerWon, string playerTime)
    {

        //se il giocatore non ha vinto...
        if (!playerWon)
        {
            //...il bottone per riprovare il livello viene attivato...
            retryButton.SetActive(true);
            //...e il testo del risultato viene cambiato con "HAI PERSO"
            endResultText.text = "YOU LOST!";

        }
        //ottiene il tempo impiegato dal giocatore per finire il circuito
        playerFinishedTime = playerTime;
        //posiziona i veicoli nel podio in base all'ordine di arrivo
        pp.PlaceVehicles();
        //fa partire la transizione
        StartCoroutine(ManageTransitions());

    }

    private IEnumerator ManageTransitions()
    {

        //FA PARTIRE LA TRANSIZIONE DI FADEIN

        //aspetta la fine della transizione
        yield return new WaitForSeconds(transitionTime);
        //attiva la schermata di fine gara
        gameObject.SetActive(true);

        //FA PARTIRE LA TRANSIZIONE DI FADEOUT

    }

}
