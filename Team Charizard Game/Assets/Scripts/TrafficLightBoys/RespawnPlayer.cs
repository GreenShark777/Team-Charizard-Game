//Riporta il giocatore all'ultimo checkpoint in cui è passato
using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    //riferimento alla linea di fine, che tiene conto dell'ultimo checkpoint in cui il giocatore è passato
    [SerializeField]
    private FinishLine finishLine = default;
    //riferimento al giudice giallo
    [SerializeField]
    private Transform player = default, //riferimento al giocatore
        yellowTrafficLightBoy = default; //riferimento al giudice giallo

    //riferimento all'Animator dell'immagine di transizione
    [SerializeField]
    private Animator fadeTransitionAnim = default;
    //riferimento alla piattaforma in cui il giudice giallo è posizionato
    private GameObject yellowBoyPlatform;
    //riferimento al punto in cui il giocatore deve voltarsi quando viene respawnato e non ha preso nessun checkpoint
    private Transform exceptionDirToFace;
    //riferimento allo script di movimento del giocatore
    private PlayerKartCtrl kartCtrl;
    //indica quanto distante deve essere rispetto al giocatore
    [SerializeField]
    private Vector3 yellowBoyOffset = default;
    //array di tutte le posizioni di respawn nella scena
    private Vector3[] respawnPositions = new Vector3[1];
    //array di direzioni, per ogni punto di respawn, in cui il giocatore deve essere girato
    private Transform[] dirToFaceAfterRespawn = new Transform[1];

    [SerializeField]
    private float allowMovingTime = 2, //indica quanto tempo deve passare prima che il giocatore possa muoversi di nuovo dopo la transizione
        transitionTime = 1; //indica quanto tempo deve durare la transizione

    //indica quanto velocemente il giudice giallo va in sù dopo aver respawnato il giocatore
    [SerializeField]
    private float flyUpSpeed = 20;
    //indica se il giocatore è stato respawnato, nel qual caso bisogna andare via
    private bool respawnedPlayer = false;


    private void Awake()
    {
        //ottiene il riferimento alla piattaforma del giudice giallo
        yellowBoyPlatform = yellowTrafficLightBoy.GetChild(0).gameObject;
        //ottiene il riferimento al punto in cui il giocatore deve voltarsi quando viene respawnato e non ha preso nessun checkpoint
        exceptionDirToFace = yellowTrafficLightBoy.GetChild(1);
        //ottiene il riferimento allo script di movimento del giocatore
        kartCtrl = player.GetComponent<PlayerKartCtrl>();

    }

    private void Start()
    {
        //sposta il punto di direzione di respawn d'eccezione esattamente davanti al giocatore(senza però metterlo nella stessa posizione)
        exceptionDirToFace.position = new Vector3(player.position.x, player.position.y, exceptionDirToFace.position.z);
        //fa in modo che il punto d'eccezione non sia più figlio del giudice giallo
        exceptionDirToFace.transform.parent = null;
        //aggiunge, come posizione in caso si cada prima di arrivare ad un qualsiasi checkpoint, la posizione iniziale del giocatore
        AddRespawnPosition(player.position, respawnPositions.Length, exceptionDirToFace.transform);


        //for (int x = 0; x < respawnPositions.Length; x++) { Debug.Log("RespawnPosition " + x + ") " + respawnPositions[x]); }

    }

    private void FixedUpdate()
    {
        //se il giocatore è stato respawnato...
        if (respawnedPlayer)
        {
            //...il giudice giallo continua a salire in sù fino a quando non è più nella visuale del giocatore
            yellowTrafficLightBoy.position = new Vector3(yellowTrafficLightBoy.position.x, yellowTrafficLightBoy.position.y + (flyUpSpeed * Time.deltaTime),
                yellowTrafficLightBoy.position.z);

        }

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
        //fa partire la transizione di fadeIn della schermata nera
        fadeTransitionAnim.SetBool("Faded", false);
        //aspetta che finisca una transizione di qualche genere, se ne esiste
        yield return new WaitForSeconds(waitBeforeTransition);
        //aspetta un po' di tempo
        yield return new WaitForSeconds(transitionTime);
        //ottiene l'indice della posizione in cui il giocatore dovrà respawnare
        int index = finishLine.GetCurrentCheckpoint();
        //se l'indice è minore di 0, il giocatore non è ancora stato in nessun checkpoint, quindi torna alla posizione iniziale
        if (index < 0) { index = respawnPositions.Length - 1; }

        Debug.Log("Respawn in: " + index);

        //fa in modo che il giocatore non sia affetto da gravità, in modo che non cada nel circuito immediatamente
        kartCtrl.IsAffectedByGravity(false);
        //mette il giocatore nella posizione di respawn più vicina
        player.position = respawnPositions[index];
        //cambia la posizione del giudice giallo, mettendolo davanti al giocatore
        yellowTrafficLightBoy.position = dirToFaceAfterRespawn[index].position + yellowBoyOffset;
        //attiva il giudice giallo
        yellowTrafficLightBoy.gameObject.SetActive(true);
        //se la sua piattaforma è disattiva, la riattiva
        if (!yellowBoyPlatform.activeSelf) { yellowBoyPlatform.SetActive(true); }
        //...se il giudice giallo non è attivo nella hierarchy, vuol dire che è ancora figlio della piattaforma disattivata, quindi lo sparenta...
        if (!yellowTrafficLightBoy.gameObject.activeInHierarchy) { yellowTrafficLightBoy.parent = null; }
        //volta il giocatore verso la posizione giusta
        player.LookAt(dirToFaceAfterRespawn[index]);
        //volta il giudice giallo verso il giocatore
        yellowTrafficLightBoy.LookAt(player);
        //fa partire la transizione di fadeOut della schermata nera
        fadeTransitionAnim.SetBool("Faded", true);
        //aspetta un po' di tempo
        yield return new WaitForSeconds(allowMovingTime);
        //il giocatore sarà nuovamente affetto da gravità
        kartCtrl.IsAffectedByGravity(true);
        //e potrà guidare di nuovo, continuando la gara
        kartCtrl.enabled = true;
        //il giudice giallo inizierà a salire
        respawnedPlayer = true;
        //aspetta un po'
        yield return new WaitForSeconds(transitionTime);
        //fa in modo che il giudice giallo salga più
        respawnedPlayer = false;
        //infine, il giudice giallo viene disattivato
        yellowTrafficLightBoy.gameObject.SetActive(false);

    }
    /// <summary>
    /// Aggiunge la posizione ricevuta all'array di posizioni in cui il giocatore può respawnare
    /// </summary>
    /// <param name="newRespawnPosition"></param>
    /// <param name="dirToFace"></param>
    /// <param name="checkpointID"></param>
    public void AddRespawnPosition(Vector3 newRespawnPosition, int checkpointID, Transform dirToFace)
    {
        //se l'ID del checkpoint che sta dando questa posizione è maggiore o uguale al numero di oggetti nell'array...
        if (checkpointID >= respawnPositions.Length || checkpointID >= dirToFaceAfterRespawn.Length)
        {
            //...crea un recipiente per le vecchie posizioni...
            var positionsRecipient = respawnPositions;
            //...dato che l'array deve essere più grande, diventa un nuovo array che può contenere tot elementi quanto l'ID del checkpoint ricevuto...
            respawnPositions = new Vector3[checkpointID + 1];
            //...e copia i valori del recipiente nel nuovo array di posizioni di respawn
            for (int pos = 0; pos < positionsRecipient.Length; pos++) { respawnPositions[pos] = positionsRecipient[pos]; }
            //...crea un recipiente per le vecchie direzioni...
            var directionsRecipient = dirToFaceAfterRespawn;
            //...dato che l'array deve essere più grande, diventa un nuovo array che può contenere tot elementi quanto l'ID del checkpoint ricevuto...
            dirToFaceAfterRespawn = new Transform[checkpointID + 1];
            //...e copia i valori del recipiente nel nuovo array di direzioni di respawn
            for (int dir = 0; dir < directionsRecipient.Length; dir++) { dirToFaceAfterRespawn[dir] = directionsRecipient[dir]; }

        }
        //...infine, aggiunge la posizione ricevuta all'array...
        respawnPositions[checkpointID] = newRespawnPosition;
        //...e la direzione in cui deve guardare
        dirToFaceAfterRespawn[checkpointID] = dirToFace;

    }

    public float GetActualRespawnTime() { return transitionTime + allowMovingTime; }

}