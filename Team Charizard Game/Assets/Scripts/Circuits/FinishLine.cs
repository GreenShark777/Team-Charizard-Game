//Si occupa di ciò che succede quando il giocatore attraversa la linea di fine
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    //array di riferimenti a tutti i checkpoint nel circuito
    //public List<Checkpoints> allCheckpoints = default;

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

    private Text finishedTimeText, //riferimento al testo che indica il tempo che ha impiegato il giocatore a finire la gara
        endResultText; //riferimento al testo che indica il risultato della gara

    //riferimento al bottone che permette di fare di nuovo lo stesso circuito
    private GameObject retryButton;

    //riferimento allo script che si occupa del comportamento del giudice verde quando viene attraversato un checkpoint o la linea di fine
    [SerializeField]
    private LapFinished lapFinished = default;
    //array di riferimenti agli script di info dei nemici
    [SerializeField]
    private EnemyCircuitInfos[] enemiesInfo = new EnemyCircuitInfos[3];

    private bool playerLost = false, //indica se un nemico ha finito la gara prima del giocatore, nel qualcaso il giocatore ha perso
        enemyAhead = false; //indica che un nemico è più avanti del giocatore nel numero di giri

    //riferimento allo script d'avviso del giudice rosso
    //[SerializeField]
    //private BackwardsTracking redBoyWarning = default;


    private void Awake()
    {
        //cambia il testo che tiene conto dei giri che il giocatore ha finito
        lapText.text = "LAP: " + currentLap + " / " + maxLapCount;
        //ottiene il riferimento ai testi da cambiare a fine la gara
        finishedTimeText = endRaceScreenUI.transform.GetChild(0).GetComponent<Text>();
        endResultText = endRaceScreenUI.transform.GetChild(1).GetComponent<Text>();
        retryButton = endRaceScreenUI.transform.GetChild(2).gameObject;
        //cicla ogni nemico nella lista e ne imposta l'ID
        for (int enemy = 0; enemy < enemiesInfo.Length; enemy++) { enemiesInfo[enemy].SetEnemyID(enemy); }

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il giocatore, si controlla se abbia effettivamente finito un giro o meno
        if (other.CompareTag("Player")) { HasFinishedLap(); }
        //se si collide con un nemico, si aggiorna il suo numero di giri
        else if (other.CompareTag("Enemy")) { EnemyFinishedLap(other.GetComponent<EnemyCircuitInfos>()); }

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
                //...e fa in modo che il primo checkpoint sia quello che deve controllare le posizioni(almeno che un nemico non è già passato prima)
                Checkpoints.SetCheckingID(0, !enemyAhead);
                enemyAhead = false;
                Debug.Log("Finito giro");
            } //altrimenti il giocatore ha completato l'ultimo giro, quindi...
            else
            {
                //...se il giocatore ha finito la gara prima di tutti i nemici...
                if (!playerLost)
                {
                    
                    Debug.Log("Vittoria!");
                }
                else //altrimenti ha perso la gara, quindi...
                {
                    //...cambia il testo di risultato di gara...
                    endResultText.text = "YOU LOST!";
                    //...e attiva il bottone per riprovare
                    retryButton.SetActive(true);
                    Debug.Log("Sconfitta!");
                }
                //...ferma il timer della corsa...
                raceTimer.enabled = false;
                //disattiva tutta la UI di gara...
                duringRaceUI.SetActive(false);
                //...aggiorna il testo del tempo nella schermata di fine gara al tempo che ha impiegato il giocatore a finire il percorso...
                finishedTimeText.text = raceTimer.GetRaceTimeText();
                //...attiva il cursore in modo che il giocatore possa cliccare i bottoni...
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                //...e attiva la schermata di fine gara
                endRaceScreenUI.SetActive(true);

            }
            //se lo script del giudice non è abilitato, lo riabilita
            if (!lapFinished.enabled) { lapFinished.enabled = true; }
            //in ogni caso, comunica al giudice verde di aver finito un giro
            lapFinished.CrossedFinishLine();
            
        }
        else //altrimenti vorrà dire che il giocatore ha provato a finire il percorso senza finire l'intero circuito
        {

            triedToCheat = true;
            Debug.Log("Non hai finito il giro -> " + currentCheckpoint + " : " + lastCheckpointID);
        }

    }

    private void EnemyFinishedLap(EnemyCircuitInfos enemy)
    {
        //comunica al nemico di aver finito un giro
        enemy.CompletedLap();
        //se il nemico ha finito l'ultimo giro, comunica che il giocatore ha perso
        if (enemy.GetEnemyLap() >= maxLapCount) { playerLost = true; }
        //altrimenti, se il giro del nemico è maggiore del giro in cui si trova il giocatore, comunica che un nemico ha finito un giro prima di lui
        else if (enemy.GetEnemyLap() > currentLap) { enemyAhead = true; }
        //imposta il checkpoint iniziale per controllare le posizioni dei kart, se il nemico è avanti
        Checkpoints.SetCheckingID(0, enemyAhead);

    }
    /// <summary>
    /// Aggiorna l'indice del checkpoint più vicino alla linea di fine, se l'indice ottenuto come parametro è più alto di quello già salvato
    /// </summary>
    /// <param name="newID"></param>
    public void UpdateLastCheckpoint(int newID)
    {
        //aggiorna il valore del checkpoint più vicino, se l'ID ricevuto è maggiore
        if (lastCheckpointID < newID) { lastCheckpointID = newID; }

    }
    /// <summary>
    /// Aggiorna l'indice dell'ultimo checkpoint che il giocatore ha oltrepassato
    /// </summary>
    /// <param name="newID"></param>
    public void UpdateCurrentPlayerCheckpoint(int newID)
    {
        //se il nuovo ID è maggiore di quello corrente, o è il primo checkpoint...
        if (newID > currentCheckpoint || newID == 0)
        {
            //...se l'ID del checkpoint passato è quello immediatamente successivo all'ultimo che era stato passato...
            bool canChangeID = (newID != lastCheckpointID || newID == currentCheckpoint + 1);
            if (canChangeID)
            {
                //...se non aveva provato, il giocatore, a barare o se questo è il primo checkpoint...
                if (!triedToCheat || newID == 0)
                {
                    //...aggiorna il valore del checkpoint corrente...
                    currentCheckpoint = newID;
                    //...comunica che il giocatore non sta barando...
                    triedToCheat = false;
                    //...e comunica al giudice verde quale checkpoint è stato passato
                    lapFinished.CrossedACheckpoint(currentCheckpoint);

                }

                //...fa sparire il giudice rosso, se era attivo
                //redBoyWarning.AdvisePlayer(false);
                //Debug.Log("CAMBIATO CURRENT CHECKPOINT: " + currentCheckpoint);

            }
            //Debug.LogError("Can Change ID: " + canChangeID);
        } //altrimenti, il giocatore sta andando al contrario, quindi...
        else
        {

            //...fa spuntare il giudice rosso che gli comunica l'errore che sta facendo
            //redBoyWarning.AdvisePlayer(true);

            Debug.LogError("Stai andando al contrario!");
        }
    
    }
    /// <summary>
    /// Ritorna l'ID dell'ultimo checkpoint da cui il giocatore è passato
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCheckpoint() { return currentCheckpoint; }
    /// <summary>
    /// Ritorna il giro in cui il kart al primo posto è arrivato(sia esso il giocatore o un nemico)
    /// </summary>
    /// <param name="wantPlayers"></param>
    /// <returns></returns>
    public int GetCurrentLap(bool wantPlayers = false) { return (!enemyAhead || wantPlayers) ? currentLap : GetHighestLap(); }
    /// <summary>
    /// Ritorna il giro più alto in cui uno dei nemici è arrivato
    /// </summary>
    /// <returns></returns>
    private int GetHighestLap()
    {
        //crea una variabile da ritornare
        int highestLap = 0;
        //controlla ogni elemento dell'array di info dei nemici e, se quel nemico è ad un giro più alto di quello già ottenuto, ne salva il valore
        foreach (EnemyCircuitInfos enemy in enemiesInfo) { if (enemy.GetEnemyLap() > highestLap) { highestLap = enemy.GetEnemyLap(); } }
        //ritorna il valore che indica il giro più alto
        return highestLap;

    }
    /// <summary>
    /// Ritorna l'array di info dei nemici
    /// </summary>
    /// <returns></returns>
    public EnemyCircuitInfos[] GetEnemiesInfos() { return enemiesInfo; }

}
