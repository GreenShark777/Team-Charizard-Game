//Si occupa di ciò che succede quando il giocatore attraversa la linea di fine
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    //indica qual'è il checkpoint più vicino alla linea di fine(quello che fa in modo che il giocatore abbia completato il giro)
    private int lastCheckpointID = -1;
    //indica a che checkpoint il giocatore sia arrivato
    private int currentCheckpoint = -1;
    //riferimento al testo che indica il giro a cui il giocatore è arrivato
    [SerializeField]
    private Text lapText = default;
    //indica a che giro è arrivato il giocatore
    private int currentLap = 1;
    //indica qual'è il massimo numero di giri da fare per vincere
    [SerializeField]
    private int maxLapCount = 3;
    //indica se il giocatore ha provato ad andare nella linea di fine senza aver prima l'intero giro del circuito
    private bool triedToCheat = false;
    //riferimento allo script che tiene conto del tempo dall'inizio della gara corsa
    [SerializeField]
    private RaceTimer raceTimer = default;
    
    [SerializeField]
    private GameObject endRaceScreenUI = default, //riferimento alla schermata di fine gara
        duringRaceUI = default; //riferimento alla schermata di gara in corso

    //riferimento al testo che indica il tempo che ha impiegato il giocatore a finire la gara
    private Text finishedTimeText;


    private void Awake()
    {
        //cambia il testo che tiene conto dei giri che il giocatore ha finito
        lapText.text = "LAP: " + currentLap + " / " + maxLapCount;
        //ottiene il riferimento al testo che indica il tempo che ha impiegato il giocatore a finire la gara
        finishedTimeText = endRaceScreenUI.transform.GetChild(0).GetComponent<Text>();

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il giocatore, si controlla se abbia effettivamente finito un giro o meno
        if (other.CompareTag("Player")) { HasFinishedLap(); }

    }
    /// <summary>
    /// Controlla se il giocatore ha finito l'intero percorso o meno
    /// </summary>
    private void HasFinishedLap()
    {
        //se l'ultimo checkpoint che il giocatore ha toccato è uguale all'ultimo checkpoint nel percorso...
        if (currentCheckpoint == lastCheckpointID)
        {
            //...aumenta il numero di giri finiti dal giocatore...
            currentLap++;
            //...se non era l'ultimo giro...
            if (currentLap - 1 != maxLapCount)
            {
                //...riporta il checkpoint corrente al valore iniziale...
                currentCheckpoint = -1;
                //...cambia il testo che tiene conto dei giri che il giocatore ha finito
                lapText.text = "LAP: " + currentLap + " / " + maxLapCount;
                Debug.Log("Finito giro");
            } //altrimenti, il giocatore ha finito la gara prima di tutti i nemici, quindi...
            else
            {
                //...ferma il timer della corsa...
                raceTimer.enabled = false;
                //disattiva tutta la UI di gara...
                duringRaceUI.SetActive(false);
                //...aggiorna il testo del tempo nella schermata di fine gara al tempo che ha impiegato il giocatore a finire il percorso...
                finishedTimeText.text = raceTimer.GetRaceTimeText();
                //...e attiva la schermata di fine gara
                endRaceScreenUI.SetActive(true);
                Debug.Log("Vittoria!");
            }
            
        }
        else //altrimenti vorrà dire che il giocatore ha provato a finire il percorso senza finire l'intero circuito
        {

            triedToCheat = true;
            Debug.Log("Non hai finito il giro -> " + currentCheckpoint + " : " + lastCheckpointID);
        }

    }

    public void UpdateLastCheckpoint(int newID)
    {
        //aggiorna il valore del checkpoint più vicino, se l'ID ricevuto è maggiore
        if (lastCheckpointID < newID) { lastCheckpointID = newID; }

    }

    public void UpdateCurrentPlayerCheckpoint(int newID)
    {
        //se il nuovo ID è maggiore di quello corrente, o è il primo checkpoint...
        if (newID > currentCheckpoint || newID == 0)
        {
            //...se l'ID del checkpoint passato è quello immediatamente successivo all'ultimo che era stato passato...
            bool canChangeID = (newID != lastCheckpointID || newID == currentCheckpoint + 1);
            if (canChangeID)
            {
                //...aggiorna il valore del checkpoint corrente, se non aveva provato il giocatore a barare o se questo è il primo checkpoint
                if (!triedToCheat || newID == 0) { currentCheckpoint = newID; triedToCheat = false; }
                //Debug.Log("CAMBIATO CURRENT CHECKPOINT: " + currentCheckpoint);
            }
            //Debug.LogError("Can Change ID: " + canChangeID);
        } //altrimenti, il giocatore sta andando al contrario, quindi...
        else
        {
            //...fa spuntare il giudice rosso che gli comunica l'errore che sta facendo

            Debug.LogError("Stai andando al contrario!");
        }
    
    }
    /// <summary>
    /// Ritorna l'ID dell'ultimo checkpoint da cui il giocatore è passato
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCheckpoint() { return currentCheckpoint; }

}
