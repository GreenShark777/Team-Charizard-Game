//Contiene le info del nemico riguardanti il circuito(giro, checkpoint, ecc...)
using UnityEngine;

public class EnemyCircuitInfos : MonoBehaviour
{
    //id del nemico, che indica anche la sua posizione nell'array di FinishLine
    private int enemyID = -1;
    //indica a quale checkpoint è arrivato
    private int crossedCheckpoint = -1;
    //indica a che giro è arrivato questo nemico
    private int lap = 1;


    public void SetEnemyID(int newID) { enemyID = newID; }

    public void SetCrossedCheckpoint(int checkpointID) { crossedCheckpoint = checkpointID; }

    public void CompletedLap() { lap++; }


    public int GetCrossedCheckpoint() { return crossedCheckpoint; }

    public int GetEnemyLap() { return lap; }

}
