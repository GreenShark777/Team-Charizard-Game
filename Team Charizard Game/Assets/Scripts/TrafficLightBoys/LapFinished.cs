using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapFinished : MonoBehaviour
{
    //riferimento alla piattaforma del giudice verde
    private GameObject greenBoyPlatform;
    
    [SerializeField]
    private Transform closeToFinishPos = default, //riferimento alla posizione in cui il giudice verde deve essere prima che il giocatore arrivi alla linea di fine
        nextToPlayerPos = default; //riferimento alla posizione in cui il giudice verde deve essere dopo che il giocatore ha completato un giro

    [SerializeField]
    private int IDForActivation = 0;

    private bool finishedLap = false;

    [SerializeField]
    private float movementSpeed = 10, 
        timeInAction = 2;


    private void Awake()
    {
        //ottiene il riferimento alla piattaforma del giudice verde
        greenBoyPlatform = transform.GetChild(0).gameObject;

    }

    private void FixedUpdate()
    {
        //se è finito un giro, sposta continuamente il giudice verde verso la posizione accanto al giocatore
        if (finishedLap) { transform.position = Vector3.Lerp(transform.position, nextToPlayerPos.position, movementSpeed * Time.deltaTime); }
        //altrimenti, se il giudice è ancora attivo, lo sposta più verso destra fino a quando non sarà più visibile dal giocatore
        else if (gameObject.activeSelf)
        { transform.position = new Vector3(transform.position.x + (movementSpeed * Time.deltaTime), transform.position.y, transform.position.z); }

    }
    /// <summary>
    /// Controlla se il checkpoint passato ha un'ID maggiore o uguale di quello indicato per attivare il giudice
    /// </summary>
    /// <param name="checkpointID"></param>
    public void CrossedACheckpoint(int checkpointID)
    {
        //se il giocatore ha superato un checkpoint con un ID maggiore o uguale a quello di attivazione...
        if (checkpointID >= IDForActivation && !gameObject.activeInHierarchy)
        {
            //...attiva il giudice verde...
            gameObject.SetActive(true);
            //...attiva la piattaforma del giudice verde se non lo è già...
            if (!greenBoyPlatform.activeSelf) { greenBoyPlatform.SetActive(true); }
            //...se il giudice verde non è attivo nella hierarchy, vuol dire che è ancora figlio della piattaforma disattivata, quindi lo sparenta...
            if (!gameObject.activeInHierarchy) { transform.parent = null; }
            //...infine, sposta il giudice nella posizione vicina alla linea di fine
            transform.position = closeToFinishPos.position;

        }

    }
    /// <summary>
    /// Comunica che il giocatore ha attraversato la linea di fine
    /// </summary>
    public void CrossedFinishLine()
    {
        //comunica che il giocatore ha finito un giro
        finishedLap = true;
        //rende il giudice verde figlio della posizione accanto al giocatore
        transform.parent = nextToPlayerPos;
        //fa partire la coroutine per seguire il giocatore e smettere dopo un po'
        StartCoroutine(FollowPlayerForABit());

    }
    /// <summary>
    /// Segue il giocatore per un po' e poi smette di seguirlo e viene disattivato
    /// </summary>
    /// <returns></returns>
    private IEnumerator FollowPlayerForABit()
    {
        //aspetta un po'
        yield return new WaitForSeconds(timeInAction);
        //comunica che il giudice verde non deve più seguire il giocatore
        finishedLap = false;
        //aspetta un altro po'
        yield return new WaitForSeconds(timeInAction);
        //disattiva il giudice verde
        gameObject.SetActive(false);
        //rende il giudice verde di nuovo figlio di nessuno
        transform.parent = null;
        //infine, disabilita questo script in modo da non far continuare erroneamente il FixedUpdate prima del dovuto
        enabled = false;

    }

}
