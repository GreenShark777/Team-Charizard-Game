//Si occupa di cosa succede quando il giocatore sta andando al contrario
using System.Collections;
using UnityEngine;

public class BackwardsTracking : MonoBehaviour
{
    
    [SerializeField]
    private Transform positionToStayToFollow = default, //riferimento al giocatore e alla posizione in cui il giudice rosso deve stare quando va al contrario
        redBoy = default; //riferimento al giudice rosso
    
    //riferimento al giocatore
    private Transform player,
        followPos;
    //riferimento alla piattaforma del giudice rosso
    private GameObject redBoyPlatform;

    private bool isWarning = false, //indica che deve avvisare il giocatore che sta andando al contrario
        stopWarning = false; //indica che non deve più avvisare il giocatore

    //indica quanto tempo deve passare prima che il giudice rosso venga disattivato(dopo che il giocatore è tornato nella direzione giusta)
    [SerializeField]
    private float deactivateAfter = 2;
    //indica quanto velocemente il giudice rosso va in sù quando non deve più avvisare il giocatore
    [SerializeField]
    private float flyUpSpeed = 30;


    private void Awake()
    {
        //ottiene il riferimento al giocatore
        player = transform.parent.GetChild(0);
        //smette di essere figlio del giocatore(in modo da non seguire la sua rotazione)
        transform.parent = null;

        followPos = transform.GetChild(0);

        followPos.parent = player;

        //ottiene il riferimento alla piattaforma del giudice rosso
        redBoyPlatform = redBoy.GetChild(0).gameObject;

    }

    private void Update()
    {
        //se deve avvisare il giocatore, gli sta costantemente davanti
        if (isWarning) { redBoy.position = Vector3.Lerp(redBoy.position, positionToStayToFollow.position, flyUpSpeed * Time.deltaTime); }
        //se non deve più avvisare il giocatore...
        if (stopWarning)
        {
            //...continua a salire in sù fino a quando non è più nella visuale del giocatore
            redBoy.position = new Vector3(positionToStayToFollow.position.x, redBoy.position.y + (flyUpSpeed * Time.deltaTime),
                positionToStayToFollow.position.z);
        
        }
        //fa rimanere questo oggetto nella posizione e con la rotazione giusta dietro il giocatore
        transform.position = followPos.position;
        transform.rotation = player.localRotation;

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
        //il giudice rosso viene disattivato
        redBoy.gameObject.SetActive(false);
        //indica che non bisogna più smettere di seguire il giocatore
        stopWarning = false;

    }

}
