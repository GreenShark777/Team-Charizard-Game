//Si occupa di mostrare i risultati della gara nella UI
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceResults : MonoBehaviour
{
    //riferimento all'Empty che contiene tutta la UI di fine gara
    private GameObject UIContainer;
    //riferimento al bottone di riprova
    [SerializeField]
    private GameObject retryButton = default;
    //riferimento allo script che si occupa di posizionare i veicoli nel podio
    [SerializeField]
    private PodioPlacement pp = default;
    //riferimento all'Animator dell'immagine di transizione
    [SerializeField]
    private Animator fadeTransitionAnim = default;

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


    //allo Start, ottiene il riferimento al contenitore della UI di fine gara
    private void Start() { UIContainer = transform.GetChild(0).gameObject; }
    /// <summary>
    /// Aggiorna i risultati di fine gara
    /// </summary>
    /// <param name="playerLost"></param>
    /// <param name="playerTime"></param>
    public void UpdateEndResults(bool playerLost, string playerTime)
    {
        //se il giocatore non ha vinto...
        if (playerLost)
        {
            //...il bottone per riprovare il livello viene attivato...
            retryButton.SetActive(true);
            //...e il testo del risultato viene cambiato con "HAI PERSO"
            endResultText.text = "YOU LOST!";

        }
        //ottiene il tempo impiegato dal giocatore per finire il circuito
        playerFinishedTime = playerTime;
        //cambia i testi delle posizioni del podio in base ai tempi delle macchine
        ChangePosTexts();
        //fa partire la transizione
        StartCoroutine(ManageTransitions());

    }
    /// <summary>
    /// Cambia i testi del tempo impiegato dai veicoli nei posti giusti
    /// </summary>
    private void ChangePosTexts()
    {
        //ottiene l'array di posizioni nel podio dei veicoli sia nemici che del giocatore
        // 0 - jeep
        // 1 - macchina volante
        // 2 - moto
        // 3 - giocatore
        var positions = Checkpoints.GetVehiclesPositions();
        //cicla ogni posizione nell'array
        for (int i = 0; i < positions.Length; i++)
        {
            //crea un riferimento locale per il testo da cambiare
            Text textToChange = null;
            //crea una stringa locale che indicherà cosa deve essere scritto nel testo da cambiare
            string newTime = "";
            //in base alla posizione che si sta controllando, ottiene il tempo di uno dei veicoli
            switch (i)
            {
                //JEEP
                case 0: { newTime = jeepFinishedTime; break; }
                //MACCHINA VOLANTE
                case 1: { newTime = flyingCarFinishedTime; break; }
                //MOTO
                case 2: { newTime = bikeFinishedTime; break; }
                //GIOCATORE
                case 3: { newTime = playerFinishedTime; break; }
                //IN CASO DI ERRORE, LO SCRIVE NELLA CONSOLE
                default: { Debug.LogError("USCITO DALL'ARRAY DI POSIZIONI"); break; }

            }
            //in base alla posizione di questo veicolo nel podio, viene cambiato un testo diverso
            switch (positions[i] - 1)
            {
                //PRIMO POSTO
                case 0: { textToChange = firstFinishedTimeText; break; }
                //SECONDO POSTO
                case 1: { textToChange = secondFinishedTimeText; break; }
                //TERZO POSTO
                case 2: { textToChange = thirdFinishedTimeText; break; }
                //QUARTO POSTO
                case 3: { textToChange = fourthFinishedTimeText; break; }
                //IN CASO DI ERRORE, LO SCRIVE NELLA CONSOLE
                default: { Debug.LogError("USCITO DALL'ARRAY DI POSIZIONI"); break; }

            }
            //infine, cambia il testo da cambiare con la nuova stringa del tempo che il veicolo ciclato ha impiegato
            textToChange.text = newTime;

        }

    }
    /// <summary>
    /// Si occupa di mostrare al momento giusto il podio e i risultati dopo la fine di transizione
    /// </summary>
    /// <returns></returns>
    private IEnumerator ManageTransitions()
    {
        //fa partire la transizione di fadeIn
        fadeTransitionAnim.SetBool("Faded", false);
        //aspetta la fine della transizione
        yield return new WaitForSeconds(transitionTime);
        //fa partire la transizione di fadeOur
        fadeTransitionAnim.SetBool("Faded", true);
        //posiziona i veicoli nel podio in base all'ordine di arrivo
        pp.PlaceVehicles();
        //attiva la UI di fine gara
        UIContainer.SetActive(true);

    }

}
