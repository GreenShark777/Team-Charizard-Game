//Si occupa di cosa succede quando il giocatore sta andando al contrario
using System.Collections;
using UnityEngine;


public class BackwardsTracking : MonoBehaviour
{
    //riferimento alla linea di fine
    [SerializeField]
    private FinishLine fl = default;

    [SerializeField]
    private Transform redBoyFollowPos = default, //riferimento al giocatore e alla posizione in cui il giudice rosso deve stare quando va al contrario
        redBoy = default; //riferimento al giudice rosso
    
    //riferimento al giocatore
    private Transform player,
        pivot; //riferimento al pivot di questo oggetto

    //riferimento alla piattaforma del giudice rosso
    private GameObject redBoyPlatform;

    private bool isWarning = false, //indica che deve avvisare il giocatore che sta andando al contrario
        stopWarning = false; //indica che non deve più avvisare il giocatore

    //indica quanto tempo deve passare prima che il giudice rosso venga disattivato(dopo che il giocatore è tornato nella direzione giusta)
    [SerializeField]
    private float deactivateAfter = 2;
    //indica quanto velocemente il giudice rosso va in sù quando non deve più avvisare il giocatore
    [SerializeField]
    private float flyUpSpeed = 30,
        //inverseRotationRate = 10, 
        rayDistance = 20;

    private void Awake()
    {
        //ottiene il riferimento al padre(che funge da pivot)
        pivot = transform.parent;
        //ottiene il riferimento al giocatore
        player = pivot.parent;
        //ottiene il riferimento alla piattaforma del giudice rosso
        redBoyPlatform = redBoy.GetChild(0).gameObject;

    }

    private void Update()
    {
        //se deve avvisare il giocatore, gli sta costantemente davanti
        if (isWarning) { redBoy.position = Vector3.Lerp(redBoy.position, redBoyFollowPos.position, flyUpSpeed * Time.deltaTime); }
        //se non deve più avvisare il giocatore...
        if (stopWarning)
        {
            //...il giudice rosso continua a salire in sù fino a quando non è più nella visuale del giocatore
            redBoy.position = new Vector3(redBoyFollowPos.position.x, redBoy.position.y + (flyUpSpeed * Time.deltaTime),
                redBoyFollowPos.position.z);
        
        }
        //crea un raycast
        RaycastHit hit;
        //se colpisce qualcosa...
        if (Physics.Raycast(player.position, player.forward, out hit, rayDistance))
        {
            //...se è un checkpoint...
            Checkpoints hitCheckpoint = hit.transform.GetComponent<Checkpoints>();
            //...controlla se è uno più indietro e avvisa il giocatore, altrimenti non lo avvisa
            if (hitCheckpoint) { AdvisePlayer(hitCheckpoint.GetThisCheckpointID() < fl.GetCurrentCheckpoint()); }
            //else { AdvisePlayer(false); }

        }

        //fa rimanere questo oggetto nella posizione giusta(dietro il giocatore), ruotando il pivot di questo oggetto al contrario da dove gira il giocatore
        //pivot.rotation = new Quaternion(0, player.rotation.y / inverseRotationRate, 0, transform.localRotation.w);
        //pivot.LookAt(checkpoint);

    }

    private void OnTriggerEnter(Collider other)
    {
        //se si collide con il punto in cui il giudice rosso si mette per avvisare il giocatore, avvisa il giocatore
        if (other.transform == redBoyFollowPos && !isWarning) { AdvisePlayer(true); }

    }

    private void OnTriggerExit(Collider other)
    {
        //se dal collider esce il collider del punto in cui il giudice rosso si mette per avvisare il giocatore, smette avvisare il giocatore
        if (other.transform == redBoyFollowPos && isWarning) { AdvisePlayer(false); }

    }


    public void AdvisePlayer(bool warnPlayer)
    {
        //se bisogna avvisare il giocatore che sta andando dalla parte sbagliata...
        if (warnPlayer)
        {
            //...se non è già stato attivato tutto...
            if (!isWarning)
            {
                //...il giudice rosso viene attivato...
                redBoy.gameObject.SetActive(true);
                //...viene attivata la piattaforma del giudice rosso, se non lo è già...
                if (!redBoyPlatform.activeSelf) { redBoyPlatform.SetActive(true); }
                //...se il giudice rosso non è attivo nella hierarchy, vuol dire che è ancora figlio della piattaforma disattivata, quindi lo sparenta...
                if (!redBoy.gameObject.activeInHierarchy) { redBoy.parent = null; }
                //...infine, comunica che sta avvisando il giocatore
                isWarning = true;

            }

        } //altrimenti, se non si deve avvisare più, fa partire la coroutine che si occupa di far sparire il giudice rosso
        else if(isWarning) { StartCoroutine(StopWarningPlayer()); }

    }

    private IEnumerator StopWarningPlayer()
    {
        //comunica che non bisogna più avvertire il giocatore
        stopWarning = true;
        //comunica che non si sta più avvisando il giocatore
        isWarning = false;
        //aspetta un po' di tempo
        yield return new WaitForSeconds(deactivateAfter);
        //se non lo deve ancora avvisare...
        if (!isWarning)
        {
            //...il giudice rosso viene disattivato...
            redBoy.gameObject.SetActive(false);
            //...indica che non bisogna più smettere di avvisare il giocatore
            stopWarning = false;

        }

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        if(player) Gizmos.DrawRay(player.position, player.forward * rayDistance);

    }

}
