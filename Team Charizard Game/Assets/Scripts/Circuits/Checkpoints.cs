//Si occupa dei checkpoint del circuito e di ciò che deve accadere quando il giocatore ci passa attraverso
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    //riferimento alla linea di fine del circuito
    [SerializeField]
    private FinishLine finishLine = default;
    //riferimento allo script di respawn
    [SerializeField]
    private RespawnPlayer respawner = default;

    //riferimento al sistema di posizionamento
    //[SerializeField]
    //private PositioningSystem posSystem = default;

    [SerializeField]
    private NewPositioningSystem newPosSystem = default;
    //riferimento alla posizione in cui il giocatore deve guardare quando viene respawnato
    private Transform directionToFace = default;
    //indica a che punto del circuito si trova questo checkpoint(più alto l'ID, più vicino è il checkpoint alla linea di fine)
    [SerializeField]
    private int checkpointID = 0;
    //indica l'ID del checkpoint che deve controllare le posizioni
    private static int checkingID = 0;
    //riferimento al giocatore
    [SerializeField]
    private Transform player = default;
    //array di riferimenti agli script di info dei nemici
    private EnemyCircuitInfos[] enemiesInfo;
    //private List<Transform> positionsToCheck = new List<Transform>();
    private Transform[] kartsPositions;
    //array di posizioni nel podio dei veicoli sia nemici che del giocatore
    // 0 - nemico 1
    // 1 - nemico 2
    // 2 - nemico 3
    // 3 - giocatore
    //[SerializeField]
    private int[] currentPositions = new int[4];
    //array che indica le posizioni calcolate dei kart
    //[SerializeField]
    int[] newCurrentPositions = new int[4];

    //[SerializeField]
    //private int[] points = new int[4];

    bool drawGizmos = false;


    private void Awake()
    {
        //aggiorna l'ID del checkpoint finale con il proprio ID
        finishLine.UpdateLastCheckpoint(checkpointID);

        //si aggiunge alla lista di checkpoint nello script della linea di fine
        //finishLine.allCheckpoints.Add(this);

        //ottiene riferimenti ai nemici
        enemiesInfo = finishLine.GetEnemiesInfos();
        //crea un indice
        int i = 0;
        //inizializza l'array di riferimenti ai kart
        kartsPositions = new Transform[4];
        //cicla ogni nemico nell'array di info dei nemici
        foreach (EnemyCircuitInfos enemy in enemiesInfo)
        {
            //aggiunge il nemico all'array di riferimenti alla posizione dei kart
            kartsPositions[i] = enemy.transform;
            //imposta la posizione iniziale del nemico
            currentPositions[i] = i + 1;
            //incrementa l'indice per il prossimo ciclo
            i++;

        }
        //aggiunge il giocatore all'array di riferimenti ai kart
        kartsPositions[3] = player;
        //aggiunge la posizione iniziale del giocatore all'array di posizioni
        currentPositions[3] = 4;
        //ottiene il riferimento alla posizione in cui il giocatore deve guardare quando viene respawnato
        directionToFace = transform.GetChild(0);
        //aggiunge la propria posizione all'array di posizioni di respawn del giocatore
        respawner.AddRespawnPosition(transform.position, checkpointID, directionToFace);



        drawGizmos = true;
    }

    private void Update()
    {
        //se questo checkpoint ha l'ID del checkpoint che deve controllare le posizioni, controlla le posizioni
        if (checkpointID == checkingID/* && Input.GetKeyDown(KeyCode.C)*/) { CheckPositions(); }

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il giocatore...
        if (other.CompareTag("Player"))
        {
            //...si aggiorna l'ultimo checkpoint su cui è passato...
            finishLine.UpdateCurrentPlayerCheckpoint(checkpointID); Debug.Log("Checkpoint: " + checkpointID);
            //...e prova a cambiare l'ID del checkpoint che deve controllare le posizioni
            SetCheckingID(checkpointID + 1);
        
        }
        //se si collide con un nemico, effettua alcuni controlli per la posizione
        else if (other.CompareTag("Enemy")) { EnemyCrossed(other.GetComponent<EnemyCircuitInfos>()); }

    }

    private void EnemyCrossed(EnemyCircuitInfos enemy)
    {
        //comunica al nemico il checkpoint a cui è arrivato
        enemy.SetCrossedCheckpoint(checkpointID);
        //prova a cambiare l'ID del checkpoint che deve controllare le posizioni
        SetCheckingID(checkpointID + 1);

    }

    private void CheckPositions()
    {
        //rimuove ogni Transform nella lista di posizioni da controllare
        //positionsToCheck.Clear();

        /*
        int index = 0;

        foreach (Transform kart in kartsPositions)
        {

            distances[index] = Vector3.Distance(transform.position, kart.position);
            
            index++;

        }
        */

        //int[] newCurrentPositions = /*currentPositions*//*(int[])currentPositions.Clone()*/new int[4];

        /*
        Debug.Log("Length:"+newCurrentPositions.Length+" -> lowerBound:"+newCurrentPositions.GetLowerBound(0));
        currentPositions.CopyTo(newCurrentPositions, 0);
        currentPositions.CopyTo(newCurrentPositions, 1);
        currentPositions.CopyTo(newCurrentPositions, 2);
        currentPositions.CopyTo(newCurrentPositions, 3);
        */

        //clona l'array di posizioni dei kart
        var v = currentPositions.Clone();
        //l'array clonato viene copiato nell'array di posizioni che verrà cambiato durante i controlli
        newCurrentPositions = (int[])v;
        //Debug.LogWarning(Equals(currentPositions, newCurrentPositions));

        //foreach (int n in currentPositions) { newCurrentPositions[m] = n; Debug.Log(newCurrentPositions[m] + " : " + n); m++; }

        /*for (int m = 0; m < 4; m++)
        {

            newCurrentPositions[m] = currentPositions[m];

        }*/

        //array di punteggio per i kart, maggiore il punteggio maggiore la sua posizione nel podio
        int[] kartPoints = new int[4];
        //cicla tutti i kart
        for (int kartX = 0; kartX < kartsPositions.Length; kartX++)
        {
            //crea un ciclo annidato per comparare tutti i kart
            for (int kartY = kartX + 1; kartY < kartsPositions.Length; kartY++)
            {
                //Debug.LogError("Controllo kart -> " + kartsPositions[kartX] + " : " + kartsPositions[kartY]);

                //crea un bool che indicherà quale dei 2 kart "vince" il controllo(chiè il più vicino alla fine della gara tra i 2)
                bool kartXWon = true;
                //ottiene i riferimenti al giro a cui entrambi i kart sono arrivati
                int lapX = (kartX != 3) ? enemiesInfo[kartX].GetEnemyLap() : finishLine.GetCurrentLap();
                int lapY = (kartY != 3) ? enemiesInfo[kartY].GetEnemyLap() : finishLine.GetCurrentLap();
                //se i 2 kart sono nello stesso giro...
                if (lapX == lapY)
                {
                    //...ottiene i riferimenti al checkpoint a cui entrambi i kart sono arrivati...
                    int checkpointX = (kartX != 3) ? enemiesInfo[kartX].GetCrossedCheckpoint() : finishLine.GetCurrentCheckpoint();
                    int checkpointY = (kartY != 3) ? enemiesInfo[kartY].GetCrossedCheckpoint() : finishLine.GetCurrentCheckpoint();
                    //...se i 2 kart hanno oltrepassato, più recentemente, lo stesso checkpoint...
                    if (checkpointX == checkpointY)
                    {
                        //...calcola la distanza tra i 2 kart...
                        float distanceX = Vector3.Distance(transform.position, kartsPositions[kartX].position);
                        float distanceY = Vector3.Distance(transform.position, kartsPositions[kartY].position);
                        //...se la distanza dal checkpoint del kartX è minore di quella del kartY, il kartX riceverà un punto
                        if (distanceX < distanceY)
                        {
                            //Debug.Log("CALCOLO DISTANZA, IL PIU' VICINO E' " + kartsPositions[kartX]);
                        }
                        else //altrimenti, il kartY riceverà un punto
                        {
                            kartXWon = false;
                            //Debug.Log("CALCOLO DISTANZA, IL PIU' VICINO E' " + kartsPositions[kartY]);
                        }

                    } //altrimenti sono su 2 checkpoint diversi, quindi...
                    else
                    {
                        //...se l'ID del checkpoint attraversato dal kartX è maggiore di quello del kartY, il kartX riceverà un punto
                        if (checkpointX > checkpointY)
                        {
                            //Debug.Log("SONO SU CHECKPOINT DIVERSI, IL MAGGIORE E' QUELLO DI " + kartsPositions[kartX]);
                        }
                        else //altrimenti, il kartY riceverà un punto
                        {
                            kartXWon = false;
                            //Debug.Log("SONO SU CHECKPOINT DIVERSI, IL MAGGIORE E' QUELLO DI " + kartsPositions[kartY]);
                        }

                    }

                } //altrimenti sono su giri diversi, quindi...
                else
                {
                    //...se il giro a cui il kartX è arrivato è maggiore di quello del kartY, il kartX riceverà un punto
                    if (lapX > lapY)
                    {
                        //Debug.Log("SONO SU GIRI DIVERSI, IL MAGGIORE E' QUELLO DI " + kartsPositions[kartX]);
                    }
                    else //altrimenti, il kartY riceverà un punto
                    {
                        kartXWon = false;
                        //Debug.Log("SONO SU GIRI DIVERSI, IL MAGGIORE E' QUELLO DI " + kartsPositions[kartY]);
                    }

                }
                //se è stato il kartX a "vincere" il controllo...
                if (kartXWon)
                {
                    //...riceverà un punto il kartX
                    kartPoints[kartX]++;
                    //Debug.Log("INCREMENTA PUNTEGGIO DI " + kartsPositions[kartX] + " A " + kartPoints[kartX]);
                }
                else //altrimenti, riceverà un punto il kartY
                {
                    kartPoints[kartY]++;
                    //Debug.Log("INCREMENTA PUNTEGGIO DI " + kartsPositions[kartY] + " A " + kartPoints[kartY]);
                }

            }

        }
        //dopo i vari controlli, cicla ogni elemento nella lista di nuove posizioni dei kart
        for (int x = 0; x < 4; x++)
        {
            //le posizioni dei kart saranno definite dai punti che hanno ottenuto nel controllo(maggiore il punteggio, più alta la posizione)
            newCurrentPositions[x] = Mathf.Abs(kartPoints[x] - 4);

            /*
            for (int y = 3; y > 0; y--)
            {

                if (kartPoints[x] > kartPoints[y])
                {
                    
                    int temp = newCurrentPositions[x];
                    newCurrentPositions[x] = newCurrentPositions[y];
                    newCurrentPositions[y] = temp;

                    //newCurrentPositions[x] = currentPositions[y];

                }
                else
                {
                    
                    int temp = newCurrentPositions[y];
                    newCurrentPositions[y] = newCurrentPositions[x];
                    newCurrentPositions[x] = temp;

                    //newCurrentPositions[y] = currentPositions[x];

                }

            }*/
            /*
            int y = x + 1;

            if (kartPoints[x] > kartPoints[y])
            {
                int temp = newCurrentPositions[x];
                newCurrentPositions[x] = newCurrentPositions[y];
                newCurrentPositions[y] = temp;
            }
            else
            {
                int temp = newCurrentPositions[y];
                newCurrentPositions[y] = newCurrentPositions[x];
                newCurrentPositions[x] = temp;
            }
            */
            /*
            switch (kartPoints[x])
            {

                case 0: { newCurrentPositions[x] = 4; break; }
                case 1: { newCurrentPositions[x] = 3; break; }
                case 2: { newCurrentPositions[x] = 2; break; }
                case 3: { newCurrentPositions[x] = 1; break; }

            }
            */

        }
        //infine, controlla se ci sono stati dei cambiamenti dall'ultimo controllo
        CheckForChanges(newCurrentPositions);

        //points = kartPoints;

        /*
        //cicla ogni nemico nella lista
        foreach (EnemyCircuitInfos enemy in enemiesInfo)
        {
            //se il nemico è tra questo checkpoint e quello immediatamente precedente e si trova nel giro più recente, viene aggiunto alla lista di posizioni da controllare
            if (enemy.GetCrossedCheckpoint() == checkpointID - 1 && enemy.GetEnemyLap() == finishLine.GetCurrentLap())
            { /*positionsToCheck.Add(enemy.transform); }

        }
        //se il giocatore è tra questo checkpoint e quello immediatamente precedente e si trova nel giro più recente, viene aggiunto alla lista di posizioni da controllare
        if (finishLine.GetCurrentCheckpoint() == checkpointID - 1 && finishLine.GetCurrentLap(true) == finishLine.GetCurrentLap())
        { /*positionsToCheck.Add(player); }
        */

    }
    /// <summary>
    /// Controlla se ci sono cambiamenti tra le posizioni salvate e quelle ricevute come parametro. Se ce ne sono, aggiorna la UI
    /// </summary>
    /// <param name="newCurrentPositions"></param>
    private void CheckForChanges(int[] newCurrentPositions)
    {
        /*
        int m = 0;
        foreach (int n in currentPositions) { Debug.Log(currentPositions[m] + " : " + n); m++; }
        m = 0;
        foreach (int n in newCurrentPositions) { Debug.Log(newCurrentPositions[m] + " : " + n); m++; }
        */

        //crea un bool che indicherà se le posizioni sono cambiate
        bool positionsChanged = false;
        //cicla ogni posizione negli array
        for (int i = 0; i < currentPositions.Length; i++)
        {
            //se l'elemento ad indice i delle posizioni salvate è diverso da quello delle posizioni ricevute...
            if (currentPositions[i] != newCurrentPositions[i])
            {
                //Debug.LogWarning("La posizione " + i + " è stata scambiata");

                //posSystem.ChangePositions((i != 3) ?  i/* - 1*/ : (i - 1));
                //positionsToChange[i] = true;
                //int temp = currentPositions[i];

                //aggiorna la posizione corrente del kart ad indice i a quella ricevuta come parametro
                currentPositions[i] = newCurrentPositions[i];
                //se l'indice è arrivato alla fine dell'array, decrementa l'indice, altrimenti lo incrementa
                if (i == 3) { i--; } else { i++; }
                //aggiorn ala posizione corrente del kart ad indice i(aggiornato) a quella ricevuta come parametro
                currentPositions[i] = newCurrentPositions[i];
                //infine, comunica che delle posizioni sono cambiate
                positionsChanged = true;

            }

        }
        //se delle posizioni sono cambiate, cambia anche la UI di posizioni
        if (positionsChanged) { newPosSystem.SetPositions(currentPositions); }

        //posSystem.NewChangePositions(currentPositions, positionsToChange);

    }

    /// <summary>
    /// Ritorna l'ID di questo checkpoint
    /// </summary>
    /// <returns></returns>
    public int GetThisCheckpointID() { return checkpointID; }
    /// <summary>
    /// Imposta l'ID del checkpoint che deve controllare le posizioni
    /// </summary>
    /// <param name="checkID"></param>
    /// <param name="reset"></param>
    public static void SetCheckingID(int checkID, bool reset = false) { checkingID = (checkID > checkingID || reset) ? checkID : checkingID; }

    private void OnDrawGizmos()
    {

        if (drawGizmos && checkingID == checkpointID)
        {

            Gizmos.DrawLine(transform.position, kartsPositions[0].position);
            Gizmos.DrawLine(transform.position, kartsPositions[1].position);
            Gizmos.DrawLine(transform.position, kartsPositions[2].position);
            Gizmos.DrawLine(transform.position, kartsPositions[3].position);

        }

    }

}