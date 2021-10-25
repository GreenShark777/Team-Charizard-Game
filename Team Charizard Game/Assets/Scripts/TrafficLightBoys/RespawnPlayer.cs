//Riporta il giocatore all'ultimo checkpoint in cui è passato
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    //riferimento alla linea di fine, che tiene conto dell'ultimo checkpoint in cui il giocatore è passato
    [SerializeField]
    private FinishLine finishLine = default;
    //riferimento al giudice giallo
    [SerializeField]
    private Transform player, //riferimento al giocatore
        yellowTrafficLightBoy = default; //riferimento al giudice giallo

    //riferimento alla piattaforma in cui il giudice giallo è posizionato
    private GameObject yellowBoyPlatform;
    //riferimento allo script di movimento del giocatore
    private PlayerKartCtrl kartCtrl;
    //indica quanto distante deve essere rispetto al giocatore
    [SerializeField]
    private Vector3 yellowBoyOffset = default;
    //lista di tutte le posizioni di respawn nella scena
    private Vector3[] respawnPositions = new Vector3[1];

    [SerializeField]
    private float allowMovingTime = 2, //indica quanto tempo deve passare prima che il giocatore possa muoversi di nuovo dopo la transizione
        transitionTime = 1; //indica quanto tempo deve durare la transizione


    private void Awake()
    {
        //ottiene il riferimento alla piattaforma del giudice giallo
        yellowBoyPlatform = yellowTrafficLightBoy.GetChild(0).gameObject;
        //ottiene il riferimento allo script di movimento del giocatore
        kartCtrl = player.GetComponent<PlayerKartCtrl>();

    }

    private void Start()
    {
        //aggiunge, come posizione in caso si cada prima di arrivare ad un qualsiasi checkpoint, la posizione iniziale del giocatore
        AddRespawnPosition(player.position, respawnPositions.Length);


        //for (int x = 0; x < respawnPositions.Length; x++) { Debug.Log("RespawnPosition " + x + ") " + respawnPositions[x]); }

    }

    private void OnTriggerEnter(Collider other)
    {
        //se il giocatore collide con questo collider, vuol dire che è caduto, quindi lo riporta dopo un po' nel circuito nella posizione dell'ultimo checkpoint che ha passato
        if (other.CompareTag("Player")) { PlayerFellOrWasDefeated(); }

    }

    /// <summary>
    /// Si occupa di ciò che deve accadere quando il giocatore viene distrutto o cade dal circuito
    /// </summary>
    public void PlayerFellOrWasDefeated()
    {
        //disabilita il movimento del giocatore
        kartCtrl.enabled = false;
        //fa partire la coroutine che si occupa delle tempistiche per gli eventi che seguono il respawn
        StartCoroutine(RepositionPlayer());

        //for (int x = 0; x < respawnPositions.Length; x++) { Debug.Log("RespawnPosition " + x + ") " + respawnPositions[x]); }

    }
    /// <summary>
    /// Si occupa delle tempistiche per gli eventi che seguono il respawn
    /// </summary>
    /// <param name="waitBeforeTransition"></param>
    /// <returns></returns>
    private IEnumerator RepositionPlayer(int waitBeforeTransition = 0)
    {

        //FA PARTIRE L'ANIMAZIONE DI SCHERMO NERO

        //aspetta un po' di tempo
        yield return new WaitForSeconds(transitionTime);
        //ottiene l'indice della posizione in cui il giocatore dovrà respawnare
        int index = finishLine.GetCurrentCheckpoint();
        //se l'indice è minore di 0, il giocatore non è ancora stato in nessun checkpoint, quindi torna alla posizione iniziale
        if (index < 0) { index = respawnPositions.Length - 1; }

        Debug.Log("Respawn in: " + index);

        //mette il giocatore nella posizione di respawn più vicina
        player.position = respawnPositions[index];
        //fa in modo che il giocatore non sia affetto da gravità, in modo che non cada nel circuito immediatamente
        kartCtrl.IsAffectedByGravity(false);
        //cambia la posizione del giudice giallo, mettendolo davanti al giocatore
        yellowTrafficLightBoy.position = player.position + yellowBoyOffset;
        //attiva il giudice giallo
        yellowTrafficLightBoy.gameObject.SetActive(true);
        //se la sua piattaforma è disattiva, la riattiva
        if (!yellowBoyPlatform.activeSelf) { yellowBoyPlatform.SetActive(true); }

        //FA PARTIRE L'ANIMAZIONE DI SCHERMO NERO AL CONTRARIO

        //aspetta un po' di tempo
        yield return new WaitForSeconds(allowMovingTime);
        //il giocatore sarà nuovamente affetto da gravità
        kartCtrl.IsAffectedByGravity(true);
        //e potrà guidare di nuovo, continuando la gara
        kartCtrl.enabled = true;

    }
    /// <summary>
    /// Aggiunge la posizione ricevuta all'array di posizioni in cui il giocatore può respawnare
    /// </summary>
    /// <param name="newRespawnPosition"></param>
    /// <param name="checkpointID"></param>
    public void AddRespawnPosition(Vector3 newRespawnPosition, int checkpointID)
    {
        //se l'ID del checkpoint che sta dando questa posizione è maggiore o uguale al numero di oggetti nell'array...
        if (checkpointID >= respawnPositions.Length)
        {
            //...crea un recipiente per le vecchie posizioni...
            var positionsRecipient = respawnPositions;
            //...dato che l'array deve essere più grande, diventa un nuovo array che può contenere tot elementi quanto l'ID del checkpoint ricevuto...
            respawnPositions = new Vector3[checkpointID + 1];
            //...e copia i valori del recipiente nel nuovo array di posizioni di respawn
            for (int i = 0; i < positionsRecipient.Length; i++) { respawnPositions[i] = positionsRecipient[i]; }

        }
        //...infine, aggiunge la posizione ricevuta all'array
        respawnPositions[checkpointID] = newRespawnPosition;

    }

}
