using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    //riferimento alla linea di fine del circuito
    [SerializeField]
    private FinishLine finishLine = default;
    //indica a che punto del circuito si trova questo checkpoint(più alto l'ID, più vicino è il checkpoint alla linea di fine)
    [SerializeField]
    private int checkpointID = 0;


    private void Awake()
    {
        //aggiorna l'ID del checkpoint finale con il proprio ID
        finishLine.UpdateLastCheckpoint(checkpointID);

    }

    private void OnTriggerEnter(Collider other)
    {
        //Se si collide con il giocatore, si aggiorna l'ultimo checkpoint su cui è passato
        if (other.CompareTag("Player")) { finishLine.UpdateCurrentPlayerCheckpoint(checkpointID); Debug.Log("Checkpoint: " + checkpointID); }

    }
    /// <summary>
    /// Ritorna l'ID di questo checkpoint
    /// </summary>
    /// <returns></returns>
    public int GetThisCheckpointID() { return checkpointID; }

}
