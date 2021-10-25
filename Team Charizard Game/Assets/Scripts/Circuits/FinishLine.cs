using System.Collections;
using System.Collections.Generic;
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
    private int currentLap = 0;
    //indica qual'è il massimo numero di giri da fare per vincere
    [SerializeField]
    private int maxLapCount = 3;


    private void Awake()
    {
        //cambia il testo che tiene conto dei giri che il giocatore ha finito
        lapText.text = "LAP: " + currentLap + " / " + maxLapCount;

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
            if (currentLap != maxLapCount)
            {
                //...riporta il checkpoint corrente al valore iniziale...
                currentCheckpoint = -1;
                //...cambia il testo che tiene conto dei giri che il giocatore ha finito
                lapText.text = "LAP: " + currentLap + " / " + maxLapCount;
                Debug.Log("Finito giro");
            } //altrimenti, il giocatore ha finito la gara prima di tutti i nemici, quindi...
            else
            {
                //...fa partire la vittoria
                Debug.Log("Vittoria!");
            }
            
        }
        else { Debug.Log("Non hai finito il giro -> " + currentCheckpoint + " : " + lastCheckpointID); }

    }

    public void UpdateLastCheckpoint(int newID)
    {
        //aggiorna il valore del checkpoint più vicino, se l'ID ricevuto è maggiore
        if (lastCheckpointID < newID) { lastCheckpointID = newID; }

    }

    public void UpdateCurrentPlayerCheckpoint(int newID)
    {
        //se il nuovo ID è maggiore di quello corrente, o il checkpoint preso è una scorciatoia...
        if (newID > currentCheckpoint)
        {
            //...se l'ID del checkpoint passato è quello immediatamente successivo all'ultimo che era stato passato, aggiorna il valore del checkpoint corrente
            bool canChangeID = newID != lastCheckpointID || newID == currentCheckpoint + 1;
            if (canChangeID) { currentCheckpoint = newID; }
            Debug.Log("Can Change ID: " + canChangeID);
        } //altrimenti, il giocatore sta andando al contrario, quindi...
        else
        {

            Debug.LogError("Stai andando al contrario!");
        }
    
    }

}
