//Si occupa del countdown per far iniziare la gara, e attiva e disattiva i vari script che devono partire solo quando il countdown è terminato
using System.Collections;
using UnityEngine;

public class RaceStartCD : MonoBehaviour
{

    [SerializeField]
    private AudioSource circuitBgMusic = default, //riferimento alla musica di background del circuito
        beepSfx = default, //riferimento al suono per il countdown
        cdEndBeepSfx = default; //riferimento al suono per la fine del countdown

    //riferimento allo script che tiene conto del tempo dall'inizio della gara corsa
    [SerializeField]
    private RaceTimer raceTimer = default;
    //riferimento allo script di movimento del kart del giocatore
    [SerializeField]
    private PlayerKartCtrl kartCtrl = default;
    //riferimento al manager delle collisioni del giocatore
    [SerializeField]
    private GameObject playerCollisionsManager = default;
    //riferimenti alle mesh dei giudici di gara(i ragazzi semaforo)
    [SerializeField]
    private SkinnedMeshRenderer redBoySMR = default,
        yellowBoySMR = default,
        greenBoySMR = default;

    //riferimenti agli Animator dei giudici di gara
    private Animator redBoyAnim,
        yellowBoyAnim,
        greenBoyAnim;

    //riferimenti ai materiali da dare ai giudici di gara per attivarli
    [SerializeField]
    private Material activeRedBoy = default,
        activeYellowBoy = default,
        activeGreenBoy = default;

    [SerializeField]
    private float startCD = 2, //indica quanto tempo bisogna aspettare prima che il giocatore possa iniziare a caricare il boost iniziale
        timeBetweenActivation = 1, //indica quanto tempo deve passare tra l'attivazione di un giudice di gara ad un altro
        flyUpSpeed = 20, //indica quanto velocemente tutti i giudici nella piattaforma salgono dopo che la corsa inizia
        activationTimer = 0.2f; //indica quanto tempo ci mettono i giudici ad attivarsi

    //indica se la gara è iniziata o meno
    private bool raceBegun = false;


    private void Awake()
    {
        //fa in modo che il giocatore non possa muoversi fino alla fine del countdown
        kartCtrl.enabled = false;
        //si disabilita, per aspettare che la cinematica di inizio gara finisca
        enabled = false;
        //ottiene i riferimenti agli Animator dei giudici di gara
        redBoyAnim = redBoySMR.GetComponentInParent<Animator>();
        yellowBoyAnim = yellowBoySMR.GetComponentInParent<Animator>();
        greenBoyAnim = greenBoySMR.GetComponentInParent<Animator>();

    }

    private void Start()
    {
        //disattiva il manager delle collisioni del giocatore, in modo che non possa subire danni o usare la sua abilità
        playerCollisionsManager.SetActive(false);
        //fa partire la coroutine per il countdown all'inizio della gara
        StartCoroutine(StartRaceCountdown());
        //viene impostato il checkpoint iniziale come checkpoint di controllo
        Checkpoints.SetCheckingID(0, true);

    }

    private void FixedUpdate()
    {
        //se la gara è iniziata...
        if (raceBegun)
        {
            //...continua a salire in sù fino a quando i giudici nella piattaforma non saranno più nella visuale del giocatore
            transform.position = new Vector3(transform.position.x, transform.position.y + (flyUpSpeed * Time.deltaTime),
                transform.position.z);

        }

    }

    /// <summary>
    /// Si occupa delle tempistiche di inizio gara
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartRaceCountdown()
    {
        //Debug.LogError("Start Countdown");
        //aspetta tot secondi
        yield return new WaitForSeconds(startCD);
        //fa partire il suono di continuo del countdown
        beepSfx.Play();
        //permette al giocatore di caricare il boost iniziale per la gara
        kartCtrl.enabled = true;
        //aspetta un altro po'
        yield return new WaitForSeconds(startCD / 2);
        //attiva il giudice di gara rosso
        StartCoroutine(ActivateTrafficLightBoy(0));
        //aspetta un certo intervallo
        yield return new WaitForSeconds(timeBetweenActivation);
        //attiva il giudice di gara giallo
        StartCoroutine(ActivateTrafficLightBoy(1));
        //aspetta un certo intervallo
        yield return new WaitForSeconds(timeBetweenActivation);
        //attiva il giudice di gara verde
        StartCoroutine(ActivateTrafficLightBoy(2));
        //aspetta finisca l'animazione di attivazione del verde
        yield return new WaitForSeconds(activationTimer);
        //attiva il timer di gara
        raceTimer.enabled = true;
        //comunica che la gara è iniziata
        raceBegun = true;
        //fa partire il suono che indica la fine del countdown
        cdEndBeepSfx.Play();
        //fa partire la musica di background del circuito
        circuitBgMusic.Play();
        //riattiva il manager delle collisioni del giocatore, in modo che possa subire danni e usare la sua abilità
        playerCollisionsManager.SetActive(true);
        //infine, comunica al giocatore che la gara è iniziata e potrà guidare
        kartCtrl.RaceBegun();
        //Debug.LogError("Race Begun");
        //aspetta un po'
        yield return new WaitForSeconds(startCD);
        //disattiva la piattaforma con i giudici
        gameObject.SetActive(false);
        //infine, disattiva quelli di prima
        yellowBoySMR.gameObject.SetActive(false);
        greenBoySMR.gameObject.SetActive(false);
        redBoySMR.gameObject.SetActive(false);
        
        //Debug.LogError("Deactivated");
    }
    /// <summary>
    /// Attiva uno dei giudici di gara in base al valore ricevuto
    /// </summary>
    /// <param name="boyToActivate"></param>
    private IEnumerator ActivateTrafficLightBoy(int boyToActivate)
    {
        //in base al valore ottenuto, attiva uno dei giudici di gara
        switch (boyToActivate)
        {
            //GIUDICE ROSSO
            case 0:
                {
                    //fa partire l'animazione del giudice rosso per accensione
                    redBoyAnim.enabled = true;
                    //aspetta che finisca l'animazione
                    yield return new WaitForSeconds(activationTimer);
                    //cambia il materiale del giudice rosso con il materiale da attivo
                    redBoySMR.material = activeRedBoy;
                    //esce dallo switch
                    break;
                
                }
            //GIUDICE GIALLO
            case 1:
                {
                    //fa partire l'animazione del giudice giallo per accensione
                    yellowBoyAnim.enabled = true;
                    //aspetta che finisca l'animazione
                    yield return new WaitForSeconds(activationTimer);
                    //cambia il materiale del giudice giallo con il materiale da attivo
                    yellowBoySMR.material = activeYellowBoy;
                    //esce dallo switch
                    break;
                
                }
            //GIUDICE VERDE
            case 2:
                {
                    //fa partire l'animazione del giudice verde per accensione
                    greenBoyAnim.enabled = true;
                    //aspetta che finisca l'animazione
                    yield return new WaitForSeconds(activationTimer);
                    //cambia il materiale del giudice verde con il materiale da attivo
                    greenBoySMR.material = activeGreenBoy;
                    //esce dallo switch
                    break;
                
                }
            //VALORE ERRATO
            default: { Debug.LogError("Valore errato, non esiste un altro giudice di gara!"); break; }

        }
        //fa partire il suono di continuo del countdown
        beepSfx.Play();

    }

}
