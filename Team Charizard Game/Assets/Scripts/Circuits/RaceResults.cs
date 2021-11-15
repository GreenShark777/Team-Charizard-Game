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

    //indica il tempo in cui il giocatore finisce il circuito
    private string playerFinishedTime;
    //indica quanto tempo deve durare la transizione
    [SerializeField]
    private float transitionTime = 2;

    [SerializeField]
    private int minOffsetTime = 10, //indica quanto tempo, al minimo, deve ricevere un nemico se ha finito la gara dopo il giocatore
        maxOffsetTime = 15; //indica quanto tempo, al massimo, deve ricevere un nemico se ha finito la gara dopo il giocatore


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
        //ottiene le info dei nemici
        var enemiesInfos = FinishLine.StaticGetEnemiesInfos();
        //ferma il timer della corsa dei nemici
        foreach (EnemyCircuitInfos enemy in enemiesInfos) { if(enemy) enemy.ActivateRaceTimer(false); }
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
        //ottiene la posizione del giocatore nel podio
        int playerPos = positions[3] - 1;
        //ottiene l'array di riferimenti alle info dei nemici
        // 0 - jeep
        // 1 - macchina volante
        // 2 - moto
        var enemiesInfos = FinishLine.StaticGetEnemiesInfos();
        //cicla ogni posizione nell'array
        for (int i = 0; i < positions.Length; i++)
        {
            //crea un riferimento locale per il testo da cambiare
            Text textToChange = null;
            //crea una stringa locale che indicherà cosa deve essere scritto nel testo da cambiare
            string newTime = "";
            //ottiene la posizione del veicolo
            int vehiclePos = positions[i] - 1;
            //in base alla posizione di questo veicolo nel podio, viene cambiato un testo diverso
            switch (vehiclePos)
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
            //in base alla posizione che si sta controllando, ottiene il tempo di uno dei veicoli
            if (i >= enemiesInfos.Length) { newTime = playerFinishedTime; }
            else
            {
                //se esiste il riferimento al nemico...
                if (enemiesInfos[i])
                {
                /*
                 * ...se il nemico è in una posizione nel podio più bassa del giocatore e non ha ancora finito di gareggiare,
                 * randomizza il suo tempo di gara altrimenti rimarrà invariato
                */
                    newTime = (vehiclePos > playerPos && !enemiesInfos[i].HasEnemyFinishedRacing()) ?
                              GetUpdatedEnemyEndRaceTime(enemiesInfos[i], vehiclePos) : enemiesInfos[i].GetEndRaceTime();

                }
            
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
    /// <summary>
    /// Aggiorna il tempo di gara del nemico, randomizzandolo in base alla sua posizione nel podio
    /// </summary>
    /// <param name="enemyInfo"></param>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    private string GetUpdatedEnemyEndRaceTime(EnemyCircuitInfos enemyInfo, int enemyPos)
    {
        //ottiene il tempo che il nemico ha impiegato in minuti, secondi e millisecondi
        int minutes = enemyInfo.GetEndRaceMinutes();
        int seconds = enemyInfo.GetEndRaceSeconds();
        int milliseconds = enemyInfo.GetEndRaceMilliseconds();
        //aggiorna i secondi e millisecondi con un numero randomico tra un minimo e un massimo moltiplicato dalla posizione del veicolo
        seconds += Random.Range(minOffsetTime * enemyPos, maxOffsetTime * enemyPos);
        milliseconds += Random.Range(minOffsetTime * enemyPos, maxOffsetTime * enemyPos);
        //se i millisecondi sono 100 o più, incrementa i secondi e diminuisce i millisecondi
        if (milliseconds >= 100) { milliseconds -= 100; seconds++; }
        //se i secondi sono 60 o più, incrementa i minuti e diminuisce i secondi
        if (seconds >= 60) { seconds -= 60; minutes++; }
        //crea la stringa da ritornare in base ai minuti, secondi e millisecondi calcolati
        string newTime = "" + minutes + ":" + seconds + ":" + milliseconds;
        //ritorna il nuovo tempo calcolato
        return newTime;

    }

}
