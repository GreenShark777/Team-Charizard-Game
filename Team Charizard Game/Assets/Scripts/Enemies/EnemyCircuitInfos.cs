//Contiene le info del nemico riguardanti il circuito(giro, checkpoint, ecc...)
using UnityEngine;

public class EnemyCircuitInfos : MonoBehaviour
{
    //riferimento al timer di gara dei nemici
    private RaceTimer rt;
    //id del nemico, che indica anche la sua posizione nell'array di FinishLine
    private int enemyID = -1;
    //indica a quale checkpoint è arrivato
    private int crossedCheckpoint = -1;
    //indica a che giro è arrivato questo nemico
    private int lap = 0;
    //indica se il nemico ha finito la gara o meno
    private bool finishedRace = false;

    private setdestination sd;

    private void Start()
    {
        
        sd = GetComponent<setdestination>();
        //sd.enabled = false;
    }

    private void Awake()
    {
        //ottiene il riferimento al timer della corsa del nemico
        rt = GetComponent<RaceTimer>();
        //disabilita all'inizio della scena il timer
        rt.enabled = false;

    }

    /// <summary>
    /// Permette di impostare l'ID del nemico
    /// </summary>
    /// <param name="newID"></param>
    public void SetEnemyID(int newID) { enemyID = newID; }
    /// <summary>
    /// Permette di impostare il checkpoint a cui questo nemico è arrivato
    /// </summary>
    /// <param name="checkpointID"></param>
    public void SetCrossedCheckpoint(int checkpointID) { crossedCheckpoint = checkpointID; }
    /// <summary>
    /// Permette di impostare il giro a cui il nemico è arrivato
    /// </summary>
    public void CompletedLap() { lap++; }
    /// <summary>
    /// Ferma il timer della corsa di questo nemico
    /// </summary>
    /// <param name="toActivate"></param>
    public void ActivateRaceTimer(bool toActivate) { rt.enabled = toActivate; }
    /// <summary>
    /// Ferma il nemico e gli comunica che ha finito la gara
    /// </summary>
    public void FinishedRacing()
    {
        //comunica che il nemico ha finito di gareggiare
        finishedRace = true;
        //ferma il timer di questo nemico
        ActivateRaceTimer(false);

        //FERMA IL NEMICO

    }

    /// <summary>
    /// Ritorna il checkpoint a cui questo nemico è arrivato
    /// </summary>
    /// <returns></returns>
    public int GetCrossedCheckpoint() { return crossedCheckpoint; }
    /// <summary>
    /// Ritorna il giro a cui questo nemico è arrivato
    /// </summary>
    /// <returns></returns>
    public int GetEnemyLap() { return lap; }
    /// <summary>
    /// Dice se il nemico ha finito la gara o meno
    /// </summary>
    /// <returns></returns>
    public bool HasEnemyFinishedRacing() { return finishedRace; }
    /// <summary>
    /// Ritorna la stringa che indica il tempo che ci ha messo il nemico a finire la gara
    /// </summary>
    /// <returns></returns>
    public string GetEndRaceTime() { return rt.GetRaceTimeText(); }
    /// <summary>
    /// Ritorna i minuti che ha impiegato il nemico a finire la gara
    /// </summary>
    /// <returns></returns>
    public int GetEndRaceMinutes() { return rt.GetMinutes(); }
    /// <summary>
    /// Ritorna i secondi che ha impiegato il nemico a finire la gara
    /// </summary>
    /// <returns></returns>
    public int GetEndRaceSeconds() { return rt.GetSeconds(); }
    /// <summary>
    /// Ritorna i millisecondi che ha impiegato il nemico a finire la gara
    /// </summary>
    /// <returns></returns>
    public int GetEndRaceMilliseconds() { return rt.GetMilliseconds(); }

    public void startEnemyRace() { sd.enabled = true; }

}
